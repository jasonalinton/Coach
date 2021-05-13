using Coach.Data.DataAccess.Logging;
using Coach.Data.EFExtention.Model.Interface;
using Coach.Model.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class coachdevEntities
    {
        public int SaveChanges(bool shouldLog)
        {
            if (shouldLog)
                return this.SaveChanges();
            else
                return base.SaveChanges();
        }

        public override int SaveChanges()
        {
            var added = new List<DbEntityEntry>();
            var modified = new List<DbEntityEntry>();
            var deleted = new List<DbEntityEntry>();

            var changedEntities = this.ChangeTracker.Entries().Where(x => x.Entity is ICoachDevModel).ToList();
            foreach (var changedEntity in changedEntities)
            {
                if (changedEntity.State == EntityState.Added)
                    added.Add(changedEntity);
                else if (changedEntity.State == EntityState.Modified)
                    modified.Add(changedEntity);
                else if (changedEntity.State == EntityState.Deleted)
                    deleted.Add(changedEntity);
            }
                
            var modifiedLogModels = new List<EventLogModel>();
            var deletedLogModels = new List<EventLogModel>();
            var addedLogModels = new List<EventLogModel>();

            if (modified.Any()) // Log info for modified must be processed before change to get the model variances
                modifiedLogModels.AddRange(LogModified(modified));
            if (deleted.Any())
                deletedLogModels.AddRange(LogDeleted(deleted));

            var @return = base.SaveChanges();

            /* This needs to be done after saving so the new IDs can be logged */
            if (added.Any())
                addedLogModels.AddRange(LogAdded(added));

            foreach (var logModel in modifiedLogModels)
                LogDAO.LogInfo(logModel);
            foreach (var logModel in deletedLogModels)
                LogDAO.LogInfo(logModel);
            foreach (var logModel in addedLogModels)
                LogDAO.LogInfo(logModel);

            return @return;
        }

        List<EventLogModel> LogAdded(List<DbEntityEntry> addedEntities)
        {
            var logModels = new List<EventLogModel>();
            foreach (var entity in addedEntities)
            {
                var model = (ICoachDevModel)entity.Entity;
                var values = this.GetPropertyValues(entity);

                if (model is IMainTable)
                {
                    var logMessage = $"{model.DisplayName} added: {model.text}";
                    logModels.Add(new EventLogModel(logMessage, model.id, model.TableName, values, "CREATE"));
                }
                else if (model is IMappingTable)
                {
                    var mappingTable = (IMappingTable)model;

                    if (mappingTable.LeftTable == null)
                        this.Entry(model).Reference(mappingTable.LeftTableName).Load();
                    else if (mappingTable.RightTable == null)
                        this.Entry(model).Reference(mappingTable.RightTableName).Load();

                    ICoachDevModel leftTable = mappingTable.LeftTable;
                    ICoachDevModel rightTable = mappingTable.RightTable;

                    var logMessage = $"{leftTable?.DisplayName ?? ""}-{rightTable?.DisplayName ?? ""} mapped: {leftTable?.text ?? ""} :: {rightTable?.text ?? ""}";
                    logModels.Add(new EventLogModel(logMessage, model.id, model.TableName, values, "CREATE"));
                }
            }
            return logModels;
        }

        List<EventLogModel> LogModified(List<DbEntityEntry> modifiedEntities)
        {
            var logModels = new List<EventLogModel>();
            foreach (var entity in modifiedEntities)
            {
                var model = (ICoachDevModel)entity.Entity;
                var values = this.GetPropertyVariences(entity);

                var logMessage = $"{model.DisplayName} modified: {model.text}";
                logModels.Add(new EventLogModel(logMessage, model.id, model.TableName, values, "UPDATE"));
            }
            return logModels;
        }

        List<EventLogModel> LogDeleted(List<DbEntityEntry> deletedEntities)
        {
            var logModels = new List<EventLogModel>();
            foreach (var entity in deletedEntities)
            {
                var model = (ICoachDevModel)entity.Entity;
                var values = this.GetPropertyValues(entity);

                var logMessage = $"{model.DisplayName} deleted: {model.text}";
                logModels.Add(new EventLogModel(logMessage, model.id, model.TableName, values, "DELETE"));
            }
            return logModels;
        }

        string GetPropertyValues(DbEntityEntry entity, bool includeNull = false)
        {
            var containsProperty = false;
            var jsonString = "{";
            foreach (var property in entity.OriginalValues.PropertyNames)
            {
                if (property == "isVisible" || property == "isActive" || property == "updatedAt" || property == "createdAt")
                    continue;
                
                var value = entity.OriginalValues[property]?.ToString();
                if (includeNull || !String.IsNullOrEmpty(value))
                {
                    if (containsProperty)
                        jsonString += ", ";
                    jsonString += $"\"{property}\":\"{value}\"";
                    containsProperty = true;
                }
            }
            return jsonString += "}";
        }

        string GetPropertyVariences(DbEntityEntry entity)
        {
            var containsVariance = false;
            var jsonString = "{";
            foreach (var property in entity.CurrentValues.PropertyNames)
            {
                if (property == "isVisible" || property == "isActive" || property == "updatedAt" || property == "createdAt")
                    continue;

                var originalValue = entity.OriginalValues[property]?.ToString();
                var currentValue = entity.CurrentValues[property]?.ToString();
                if (originalValue != currentValue)
                {
                    if (containsVariance)
                        jsonString += ", ";
                    jsonString += $"\"{property}\":\"{currentValue} :: {originalValue}\"";
                    containsVariance = true;
                }
            }

            if (containsVariance)
                return jsonString += "}";
            else
                return "";
        }
    }
}
