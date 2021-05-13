using Coach.Data.DataAccess.Briefing;
using Coach.Data.DataAccess.Inventory;
using Coach.Data.DataAccess.Inventory.Physical;
using Coach.Data.DataAccess.Items;
using Coach.Data.DataAccess.Items.Todo;
using Coach.Service.Briefing;
using Coach.Service.Inventory.Physical;
using Coach.Service.Items;
using Coach.Service.Items.Todo;
using Coach.Service.Planner;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coach.Site.Infrustructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            /* Service Bindings */
            _kernel.Bind<IGoalService>().To<GoalService>();
            _kernel.Bind<IRoutineService>().To<RoutineService>();
            _kernel.Bind<ITodoService>().To<TodoService>();
            _kernel.Bind<IPlannerService>().To<PlannerService>();
            _kernel.Bind<IMobilePlannerService>().To<MobilePlannerService>();
            _kernel.Bind<IGoogleTaskService>().To<GoogleTaskService>();
            _kernel.Bind<IBriefingService>().To<BriefingService>();
            _kernel.Bind<ISleepService>().To<SleepService>(); 

            /* Data Access Bindings */
            _kernel.Bind<IInventoryDAO>().To<InventoryDAO>();
            _kernel.Bind<IGoalDAO>().To<GoalDAO>();
            _kernel.Bind<IRoutineDAO>().To<RoutineDAO>();
            _kernel.Bind<ITodoDAO>().To<TodoDAO>();
            _kernel.Bind<IGoogleTaskDAO>().To<GoogleTaskDAO>();
            _kernel.Bind<IBriefingDAO>().To<BriefingDAO>();
            _kernel.Bind<ISleepDAO>().To<SleepDAO>();
        }
    }
}