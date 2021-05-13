using Coach.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Mobile.Data.Repository
{
    public class CoachRepository
    {
        public CoachRepositoryContext RepositoryContext;

        public CoachRepository()
        {
            this.RepositoryContext = CoachRepositoryContext.Instance;
        }

        public static CoachRepository Instance { get; } = new CoachRepository();

        //public virtual async Task<ICoachModel> GetModel(string modelName, string remoteID)
        //{
        //    return await this.RepositoryContext.GetModel(modelName, remoteID);
        //}

        public virtual async Task<List<ICoachModel>> GetModels(string modelName)
        {
            return await this.RepositoryContext.GetModels(modelName);
        }

        public virtual async Task AddModel(ICoachModel CoachModel)
        {
            await this.AddModels(new List<ICoachModel> { CoachModel });
        }

        public virtual async Task AddModels(List<ICoachModel> CoachModels)
        {
            await this.RepositoryContext.AddModels(CoachModels);
        }

        public virtual async Task UpdateModel(ICoachModel CoachModel)
        {
            await this.RepositoryContext.UpdateModel(CoachModel);
        }

        public virtual async Task UpdateModels(List<ICoachModel> CoachModels)
        {
            await this.RepositoryContext.UpdateModels(CoachModels);
        }

        public virtual void DeleteModel(ICoachModel CoachModel)
        {
            this.RepositoryContext.DeleteModel(CoachModel);
        }

        public virtual void DeleteModels(List<ICoachModel> CoachModels)
        {
            this.RepositoryContext.DeleteModels(CoachModels);
        }

        public virtual Task<int> Save()
        {
            throw new NotImplementedException();
        }



        /*-------------------------------------------------------------------------------------------------*/
        /*--------------------------------------------- Misc ----------------------------------------------*/
        /*-------------------------------------------------------------------------------------------------*/

        //public virtual async Task<ICoachModel> GetTechnician(string emailAddress)
        //{
        //    return await this.RepositoryContext.GetTechnician(emailAddress);
        //}

        //public virtual async Task<Dictionary<string, List<ICoachModel>>> GetWorkOrderDispatches_forEmployee(int employeeID)
        //{
        //    return await this.RepositoryContext.GetWorkOrderDispatches_forEmployee(employeeID);
        //}

        //public virtual async Task<List<ICoachModel>> GetEquipmentComponents_forDispatch(string WONum)
        //{
        //    return await this.RepositoryContext.GetEquipmentComponents_forDispatch(WONum);
        //}
    }
}
