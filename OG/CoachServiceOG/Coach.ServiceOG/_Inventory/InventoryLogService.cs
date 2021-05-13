
using AutoMapper;
using Coach.Data.Mappping;
using Coach.ServiceOG._Data._CoachModel;
using CoachModel._Inventory._Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Service._Inventory
{
    public class InventoryLogService
    {
        coachogEntities entities;
        IMapper CoachMapper;

        public InventoryLogService()
        {
            this.entities = new coachogEntities();

            

                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<char, int>().ConvertUsing(c => Convert.ToInt32(c));
                    cfg.CreateMap<string, int>().ConvertUsing(s => Convert.ToInt32(s));
                    cfg.CreateMap<string, double>().ConvertUsing(s => Convert.ToDouble(s));
                    

                    #region OG
                    /* Log Item */
                    cfg.CreateMap<logitemfield_view, LogItem>()
                    .ForMember(g => g.InventoryItemName, m => m.MapFrom(g_v => g_v.inventoryItem))
                    .ForMember(g => g.Name, m => m.MapFrom(g_v => g_v.logItem))
                    .ForMember(g => g.Position, m => m.MapFrom(g_v => g_v.position_LogItem))
                    .ForMember(g => g.IsActive, m => m.MapFrom(g_v => g_v.isActive_LogItem))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                    /* Log Item Field */
                    cfg.CreateMap<logitemfield_view, ItemField>()
                        .ForMember(g => g.idType, m => m.MapFrom(g_v => g_v.idType_Field))
                        .ForMember(g => g.idType_Data, m => m.MapFrom(g_v => g_v.idType_Data))
                        .ForMember(g => g.Name, m => m.MapFrom(g_v => g_v.field))
                        .ForMember(g => g.DateType, m => m.MapFrom(g_v => g_v.dataType))
                        .ForMember(g => g.Type, m => m.MapFrom(g_v => g_v.fieldType))
                        .ForMember(g => g.Position, m => m.MapFrom(g_v => g_v.position_Field))
                        .ForMember(g => g.IsActive, m => m.MapFrom(g_v => g_v.isActive_Field))
                        .IgnoreAllPropertiesWithAnInaccessibleSetter();

                    /* Log Item Field Value */
                    cfg.CreateMap<logentry_logitemfield, FieldValue>()
                        .IgnoreAllPropertiesWithAnInaccessibleSetter();
                    #endregion
                });

            CoachMapper = config.CreateMapper();
        }

        public List<LogItem> GetLogItems()
        {
            List<logitemfield_view> logitemfield_views = entities.logitemfield_view
                .Where(x => x.isActive_LogItem == true).ToList()
                .OrderBy(x => x.position_Field)
                .OrderBy(x => x.position_LogItem)
                .OrderBy(x => x.position_InventoryItem)
                .ToList();

            var LogItems = this.CreateLogItemModels(logitemfield_views);
            return LogItems;
        }

        public List<LogItem> CreateLogItemModels(List<logitemfield_view> logitemfield_views)
        {
            var LogItems = new List<LogItem>();

            foreach (var logitemfield_view in logitemfield_views)
            {
                LogItem LogItem;
                if (LogItems.Select(x => x.idLogItem).Contains((int)logitemfield_view.idLogItem)) // If log item was already created 
                {
                    LogItem = LogItems.SingleOrDefault(x => x.idLogItem == logitemfield_view.idLogItem); // Get log item
                }
                else
                {
                    //LogItem = new LogItem(); // Create log item
                    //Mapper.Map(logitemfield_view, LogItem);
                    LogItem = CoachMapper.Map<LogItem>(logitemfield_view);
                    LogItems.Add(LogItem); // Add new log item to collection
                }

                /* Creat new Field */
                //var Field = new ItemField();
                //Mapper.Map(logitemfield_view, Field);
                var Field = CoachMapper.Map<ItemField>(logitemfield_view);
                LogItem.Fields.Add(Field);

                var logentry_logitemfields = entities.logentry_logitemfield
                    .Where(x => x.idLogItemField == logitemfield_view.idLogItemField).ToList();

                foreach (var logentry_logitemfield in logentry_logitemfields)
                {
                    var FieldValue = new FieldValue
                    {
                        idField = logentry_logitemfield.idLogItemField,
                        idEntry = logentry_logitemfield.idLogEntry,
                        idLogEntry_LogItemField = logentry_logitemfield.idLogEntry_LogItemField,
                        Value = logentry_logitemfield.value,
                        WasSaved = true,
                        DateTime = logentry_logitemfield.logentry.dateTime
                    };
                    Field.Values.Add(FieldValue);
                }
            }

            return LogItems;
        }

        public void LogLogItem(LogItem logItem)
        {
            var logentry = new logentry
            {
                idLogItem = logItem.idLogItem,
                dateTime = DateTime.Now
            };
            entities.logentries.Add(logentry);

            foreach (var logItemField in logItem.Fields)
            {
                var logentry_logitemfield = new logentry_logitemfield
                {
                    logentry = logentry,
                    idLogItemField = logItemField.idLogItemField,
                    value = logItemField.Value
                };
                entities.logentry_logitemfield.Add(logentry_logitemfield);
            }
            entities.SaveChanges();
        }

        public void LogLogItems(List<LogItem> logItems)
        {
            foreach (var logItem in logItems)
            {
                this.LogLogItem(logItem);
            }
        }

        public void UpdateLogItem(LogItem logItem)
        {
            foreach (var logItemField in logItem.Fields)
            {
                var logentry_logitemfield = entities.logentry_logitemfield.ToList().Last(x => x.idLogItemField == logItemField.idLogItemField);
                logentry_logitemfield.value = logItemField.Value;

                //foreach (var fieldValue in logItemField.Values.Where(x => x.WasEdited).ToList())
                //{
                //    //var logentry_logitemfield = entities.logentry_logitemfield.Single(x => x.idLogEntry_LogItemField == fieldValue.idLogEntry_LogItemField);
                    
                //    //logentry_logitemfield.updatedAt = DateTime.Now;

                //    //logentry_logitemfield.logentry.dateTime = fieldValue.DateTime;
                //    //logentry_logitemfield.logentry.updatedAt = DateTime.Now;

                //    entities.Entry(logentry_logitemfield).State = EntityState.Modified;
                //}
            }
            entities.SaveChanges();
        }
    }
}
