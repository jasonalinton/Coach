using AutoMapper;
using Coach.Data.Model;
using Coach.Model.Briefing;
using Coach.Model.Helper;
using Coach.Model.Inventory.Physical.Sleep;
using Coach.Model.Items;
using Coach.Model.Items.GoogleTask;
using Coach.Model.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryItemModel = Coach.Model.Items.InventoryItemModel;

namespace Coach.Data.Mappping
{
    public class MyMapper
    {
        public static IMapper CoachMapper { get; set; }

        public static void CreateMaps()
        {
            
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<string, List<string>>().ConvertUsing<StringToStringList>();
                cfg.CreateMap<string, List<int>>().ConvertUsing<StringToIntList>();
                cfg.CreateMap<string, TimeSpan>().ConvertUsing<StringToTimeSpan>();
                cfg.CreateMap<char, int>().ConvertUsing(c => Convert.ToInt32(c));
                cfg.CreateMap<string, int>().ConvertUsing(s => Convert.ToInt32(s));
                cfg.CreateMap<string, double>().ConvertUsing(s => Convert.ToDouble(s));

                #region DataAccess to Model
                /* Items */
                cfg.CreateMap<inventoryitem, InventoryItemModel>();
                cfg.CreateMap<goal_view, GoalModel>();
                cfg.CreateMap<goal, GoalModel>();
                cfg.CreateMap<todo, TodoModel>()
                    .ForMember(dest => dest.InventoryItems, y => y.MapFrom(src => src.inventoryitem_todo.Select(x => x.inventoryitem)))
                    .ForMember(dest => dest.Goals, y => y.MapFrom(src => src.goal_todo.Select(x => x.goal)))
                    .ForMember(dest => dest.Routines, y => y.MapFrom(src => src.routine_todo.Select(x => x.routine)))
                    .ForMember(dest => dest.Parents, y => y.MapFrom(src => src.todo_parent.Select(x => x.parent)))
                    .ForMember(dest => dest.Children, y => y.MapFrom(src => src.todo_child.Select(x => x.child)))
                    .ForMember(dest => dest.ParentPosition, y => y.MapFrom(src => src.todo_child.FirstOrDefault().position))
                    .ForMember(dest => dest.RoutinePosition, y => y.MapFrom(src => src.routine_todo.FirstOrDefault().position))
                    .ForMember(dest => dest.EventTasks, y => y.MapFrom(src => src.todo_eventtask.Select(x => x.eventtask)));
                cfg.CreateMap<todo_view, TodoModel>();
                cfg.CreateMap<routine, RoutineModel>()
                    .ForMember(dest => dest.Todos, y => y.MapFrom(src => src.routine_todo.Select(x => x.todo)))
                    .ForMember(dest => dest.EventTasks, y => y.MapFrom(src => src.routine_eventtask.Select(x => x.eventtask)));
                cfg.CreateMap<routine_view, RoutineModel>();
                cfg.CreateMap<eventtask, EventTaskModel>();

                cfg.CreateMap<sleepsession, SleepSessionModel>();
                cfg.CreateMap<sleeprequirement, SleepRequirementModel>();

                cfg.CreateMap<briefing, BriefingModel>()
                    .ForMember(dest => dest.InventoryItems, y => y.MapFrom(src => src.inventoryitem_briefing.Select(x => x.inventoryitem)))
                    .ForMember(dest => dest.Types, y => y.MapFrom(src => src.briefing_type.Select(x => x.type)))
                    .ForMember(dest => dest.InventoryItemID, y => y.MapFrom(src => src.inventoryitem_briefing.ToList()[0].idInventoryItem))
                    .ForMember(dest => dest.TypeID, y => y.MapFrom(src => src.briefing_type.ToList()[0].idType));

                /* Google Task API */
                cfg.CreateMap<Google.Apis.Tasks.v1.Data.Task, GoogleTaskModel>();
                cfg.CreateMap<Google.Apis.Tasks.v1.Data.Task.LinksData, GoogleTaskModel.LinksData>();

                /* Universal */
                cfg.CreateMap<type, TypeModel>();

                /* Logging */
                cfg.CreateMap<eventlog, EventLogModel>();
                cfg.CreateMap<errorlog, ErrorLogModel>();
                #endregion

                #region Model to DataAccess
                /* Items */
                cfg.CreateMap<InventoryItemModel, inventoryitem>();
                cfg.CreateMap<GoalModel, goal>();
                cfg.CreateMap<TodoModel, todo>();
                cfg.CreateMap<RoutineModel, routine>();
                cfg.CreateMap<RoutineModel, routine_view>();
                cfg.CreateMap<EventTaskModel, eventtask>();

                cfg.CreateMap<SleepSessionModel, sleepsession>();
                cfg.CreateMap<SleepRequirementModel, sleeprequirement>();

                cfg.CreateMap<BriefingModel, briefing>();

                /* Google Task API */
                cfg.CreateMap<GoogleTaskModel, Google.Apis.Tasks.v1.Data.Task>();
                cfg.CreateMap<GoogleTaskModel.LinksData, Google.Apis.Tasks.v1.Data.Task.LinksData>();

                /* Universal */
                cfg.CreateMap<TypeModel, type>();

                /* Logging */
                cfg.CreateMap<EventLogModel, eventlog>();
                cfg.CreateMap<ErrorLogModel, errorlog>();

                /* Sleep */
                cfg.CreateMap<AutoSleepModel, sleepsession>()
                    .ForMember(dest => dest.date, y => y.MapFrom(src => src.Date))
                    .ForMember(dest => dest.timeInBed, y => y.MapFrom(src => src.Bedtime))
                    .ForMember(dest => dest.timeAwake, y => y.MapFrom(src => src.Waketime))
                    .ForMember(dest => dest.duration, y => y.MapFrom(src => src.InBed))
                    .ForMember(dest => dest.durationAsleep, y => y.MapFrom(src => src.Asleep))
                    .ForMember(dest => dest.durationFallingAsleep, y => y.MapFrom(src => src.FellAsleepIn))
                    .ForMember(dest => dest.durationQualitySleep, y => y.MapFrom(src => src.Quality))
                    .ForMember(dest => dest.durationDeepSleep, y => y.MapFrom(src => src.Deep))
                    .ForMember(dest => dest.durationAwake, y => y.MapFrom(src => src.Awake))
                    .ForMember(dest => dest.wakingHeartRate, y => y.MapFrom(src => src.WakingBPM))
                    .ForMember(dest => dest.averageHeartRate, y => y.MapFrom(src => src.SleepBPM));
                #endregion
            });

            CoachMapper = config.CreateMapper();
        }

        public class StringToStringList : ITypeConverter<string, List<string>>
        {
            public List<string> Convert(string source, List<string> destination, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source))
                    return new List<string>();
                else
                    return source.Split(',').ToList();
            }
        }

        public class StringToIntList : ITypeConverter<string, List<int>>
        {
            public List<int> Convert(string source, List<int> destination, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source))
                    return new List<int>();
                else
                    return source.Split(',').Select(int.Parse).ToList();
            }
        }

        public class StringToTimeSpan : ITypeConverter<string, TimeSpan>
        {
            public TimeSpan Convert(string source, TimeSpan destination, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source))
                    return new TimeSpan();
                else
                {
                    var timespanString = source.Split(':');
                    var timespan = Array.ConvertAll(timespanString, x => System.Convert.ToInt32(x));
                    return new TimeSpan(timespan[0], timespan[1], timespan[2]);
                }
            }
        }
    }
}
