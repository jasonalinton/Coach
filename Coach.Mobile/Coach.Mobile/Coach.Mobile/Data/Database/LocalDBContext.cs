using Coach.Mobile.Data.Packet;
using Coach.Mobile.Models;
using Coach.Mobile.Models.Briefing;
using Coach.Mobile.Models.Helper;
using Coach.Mobile.Models.InventoryItem;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Mobile.Data.Database
{
    public class LocalDBContext
    {
        /* The TodoItemDatabase uses the .NET Lazy class to delay initialization of the database until it's first accessed. 
         * Using lazy initialization prevents the database loading process from delaying the app launch. For more information, see Lazy<T> Class. */
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(LocalDatabaseConstants.DatabasePath, LocalDatabaseConstants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized;

        public LocalDBContext()
        {
            Initialize().SafeFireAndForget(false);
        }
        /// <summary>
        /// Initialize SQLite Tables
        /// </summary>
        /// <returns></returns>
        async Task Initialize()
        {
            if (!initialized)
            {
                Type[] models = {
                    typeof(InventoryItemModel),
                    typeof(TypeModel),
                    typeof(BriefingModel)
                };

                foreach (var model in models)
                {
                    if (!Database.TableMappings.Any(x => x.MappedType.Name == model.Name))
                        await Database.CreateTablesAsync(CreateFlags.None, model).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public async Task<ICoachPacket> GetModels(string modelName)
        {
            List<ICoachModel> iCoachModels;

            if (modelName == "InventoryItemModel")
            {
                var models = await Database.Table<InventoryItemModel>().ToListAsync();
                iCoachModels = models.Cast<ICoachModel>().ToList();
            }
            else if (modelName == "TypeModel")
            {
                var models = await Database.Table<TypeModel>().ToListAsync();
                iCoachModels = models.Cast<ICoachModel>().ToList();
            }
            else if (modelName == "BriefingModel")
            {
                var models = await Database.Table<BriefingModel>().ToListAsync();
                iCoachModels = models.Cast<ICoachModel>().ToList();
            }
            else
            {
                return null;
            }

            return CoachPacket.CreatePacket(iCoachModels);
        }

        public async Task<ICoachPacket> AddModel(ICoachModel model)
        {
            return await AddModels(new List<ICoachModel> { model });
        }

        public async Task<ICoachPacket> AddModels(List<ICoachModel> models)
        {
            foreach (var model in models)
                model.SQLiteID = await Database.InsertAsync(model);

            return CoachPacket.CreatePacket(models);
        }

        public async Task UpdateModel(ICoachModel model)
        {
            await Database.UpdateAsync(model);
        }

        public async Task UpdateModels(List<ICoachModel> models)
        {
            await Database.UpdateAllAsync(models);
        }

        public async Task DeleteModel(ICoachModel model)
        {
            await Database.DeleteAsync(model);
        }

        public async Task DeleteModels(List<ICoachModel> models)
        {
            foreach (var model in models)
                await Database.DeleteAsync(model);
        }

        public static async Task ClearDatabase()
        {
            var tableMappings = Database.TableMappings;
            foreach (var mapping in tableMappings)
            {
                await Database.DropTableAsync(mapping);
            }
        }
    }

    public static class LocalDatabaseConstants
    {
        public const string DatabaseFilename = "Coach";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }

    /* In order to start the asynchronous initialization, avoid blocking execution, 
     * and have the opportunity to catch exceptions, the sample application uses an extension method called SafeFireAndForget. 
     * The SafeFireAndForget extension method provides additional functionality to the Task class */
    public static class TaskExtension
    {
        // NOTE: Async void is intentional here. This provides a way
        // to call an async method from the constructor while
        // communicating intent to fire and forget, and allow
        // handling of exceptions
        public static async void SafeFireAndForget(this Task task, bool returnToCallingContext, Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }

            // if the provided action is not null, catch and
            // pass the thrown exception
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
}
