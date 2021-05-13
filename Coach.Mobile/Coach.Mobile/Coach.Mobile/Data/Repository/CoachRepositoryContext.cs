using Coach.Mobile.Data.Database;
using Coach.Mobile.Data.Packet;
using Coach.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Mobile.Data.Repository
{
    // MAKE THIS SINGLETON!!
    public class CoachRepositoryContext
    {
        private bool isConnectedToRemote;

        public LocalDBContext LocalDBContext = new LocalDBContext();
        //public RemoteDBContext RemoteDBContext = new RemoteDBContext();

        public CoachRepositoryContext()
        {

        }

        public static CoachRepositoryContext Instance { get; } = new CoachRepositoryContext();

        public bool ShouldDisconnectFromRemote { get; set; }

        public bool IsConnectedToRemote
        {
            get
            {
                if (this.ShouldDisconnectFromRemote == false)
                {
                    this.isConnectedToRemote = true;
                    return this.isConnectedToRemote;
                }
                else
                {
                    this.isConnectedToRemote = false;
                    return this.isConnectedToRemote;
                }
            }
        }

        /* 
         * TypeModel
         * InventoryItemModel
         * BriefingModel
         */

        public async void AddModel(ICoachModel CoachModel)
        {
            /* First - Save CoachModel to Local */
            var CoachPacket = await this.LocalDBContext.AddModel(CoachModel);

            ///* Then - Try to Save CoachModel to Remote */
            //if (this.IsConnectedToRemote == true)
            //{
            //    /* Returned model might be different than model going in */
            //    CoachPacket = await this.RemoteDBContext.Add(CoachPacket.CoachModel);
            //    CoachModel = CoachPacket.CoachModel;

            //    /* Lastly - Update to Local 
            //     * This will save the RemoteID and any syncronization updates to the database */
            //    await this.LocalDBContext.UpdateModel(CoachModel);
            //}
        }

        public async Task AddModels(List<ICoachModel> CoachModels)
        {
            /* First - Save CoachModel to Local */
            var CoachPacket = await this.LocalDBContext.AddModels(CoachModels);

            ///* Then - Try to Save CoachModel to Remote */
            //if (this.IsConnectedToRemote == true)
            //{
            //    /* Returned models might be different than models going in */
            //    CoachPacket = await this.RemoteDBContext.Add(CoachPacket.CoachModels);

            //    // Second - Save to Local
            //    await this.LocalDBContext.AddModels(CoachPacket.CoachModels);
            //}
        }

        //public async Task<ICoachModel> GetModel(string modelName, string remoteID)
        //{
        //    ICoachModel CoachModel;

        //    if (this.IsConnectedToRemote == true)
        //    {
        //        ICoachPacket CoachPacket = await this.RemoteDBContext.Get(modelName, remoteID);
        //        CoachModel = CoachPacket.CoachModels[0];

        //        await this.LocalDBContext.UpdateModel(CoachModel); // Update Local Models to match Remote Models received
        //    }
        //    else
        //    {
        //        ICoachPacket CoachPacket = await this.LocalDBContext.GetModel(modelName, remoteID);
        //        CoachModel = CoachPacket.CoachModels[0];
        //    }

        //    return CoachModel;
        //}

        public async Task<List<ICoachModel>> GetModels(string modelName)
        {
            List<ICoachModel> CoachModels;

            //if (this.IsConnectedToRemote == true)
            //{
            //    ICoachPacket CoachPacket = await this.RemoteDBContext.Get(modelName);
            //    CoachModels = CoachPacket.CoachModels;

            //    await this.LocalDBContext.UpdateModels(CoachModels); // Update Local Models to match Remote Models received
            //}
            //else
            //{
                ICoachPacket CoachPacket = await this.LocalDBContext.GetModels(modelName);
                CoachModels = CoachPacket.CoachModels;
            //}

            return CoachModels;
        }

        public async Task UpdateModel(ICoachModel CoachModel)
        {
            ///* Then - Try to Update CoachModel in Remote */
            //if (this.IsConnectedToRemote == true)
            //{
            //    /* Returned model might be different than model going in */
            //    ICoachPacket CoachPacket = await this.RemoteDBContext.Update(CoachPacket.CoachModel);
            //    CoachModel = CoachPacket.CoachModel;
            //}

            /* Second - Save to Local */
            await this.LocalDBContext.UpdateModel(CoachModel);
        }

        public async Task UpdateModels(List<ICoachModel> CoachModels)
        {
            ///* First - Try to Update CoachModel in Remote */
            //if (this.IsConnectedToRemote == true)
            //{
            //    ///* Returned model might be different than model going in */
            //    //ICoachPacket CoachPacket = await this.RemoteDBContext.Update(CoachModels);
            //    //CoachModels = CoachPacket.CoachModels;
            //}

            /* Second - Save to Local */
            await this.LocalDBContext.UpdateModels(CoachModels);
        }

        public async void DeleteModel(ICoachModel CoachModel)
        {
            ///* First - Try to Save CoachModel to Remote */
            //if (this.IsConnectedToRemote == true)
            //{
            //    ICoachPacket CoachPacket = await this.RemoteDBContext.Delete(CoachModel);
            //    CoachModel = CoachPacket.CoachModels[0];
            //}

            /* Second - Delete from Local */
            await this.LocalDBContext.DeleteModel(CoachModel);
        }

        public async void DeleteModels(List<ICoachModel> CoachModels)
        {
            ///* First - Try to Save CoachModel to Remote */
            //if (this.IsConnectedToRemote == true)
            //{
            //    /* Returned model might be different than model going in */
            //    ICoachPacket CoachPacket = await this.RemoteDBContext.Delete(CoachModels);
            //    CoachModels = CoachPacket.CoachModels;
            //}

            /* Second - Delete from Local */
            await this.LocalDBContext.DeleteModels(CoachModels);
        }


        /*-------------------------------------------------------------------------------------------------*/
        /*------------------------------------------- Misc ------------------------------------------------*/
        /*-------------------------------------------------------------------------------------------------*/

        //public async Task<ICoachModel> GetTechnician(string emailAddress)
        //{
        //    ICoachPacket CoachPacket = null;
        //    ICoachModel CoachModel = null;

        //    if (this.IsConnectedToRemote == true)
        //    {
        //        CoachPacket = await this.RemoteDBContext.GetTechnician(emailAddress);
        //        CoachModel = CoachPacket.CoachModels[0];

        //        await this.LocalDBContext.UpdateModel(CoachModel);

        //        return CoachModel;
        //    }
        //    else
        //    {
        //        CoachPacket = await this.LocalDBContext.GetTechnician(emailAddress);
        //        CoachModel = CoachPacket.CoachModels[0];

        //        return CoachModel;
        //    }
        //}


        /*-------------------------------------------------------------------------------------------------*/
        /*------------------------------------------ Views ------------------------------------------------*/
        /*-------------------------------------------------------------------------------------------------*/

        //public async Task<Dictionary<string, List<ICoachModel>>> GetWorkOrderDispatches_forEmployee(int employeeID)
        //{
        //    if (this.IsConnectedToRemote == true)
        //    {
        //        WorkOrderDispatchPacket WorkOrderDispatchPacket = (WorkOrderDispatchPacket)await this.RemoteDBContext.GetWorkOrderDispatch_forEmployee(employeeID);

        //        Dictionary<string, List<ICoachModel>> CoachModelDictionary = new Dictionary<string, List<ICoachModel>>()
        //        {
        //            { "Customer", WorkOrderDispatchPacket.Customers.Cast<ICoachModel>().ToList() },
        //            { "WorkOrderDispatch", WorkOrderDispatchPacket.WorkOrderDispatches.Cast<ICoachModel>().ToList() },
        //        };

        //        return CoachModelDictionary;
        //    }

        //    return null;
        //}

        //public async Task<List<ICoachModel>> GetEquipmentComponents_forDispatch(string WONum)
        //{
        //    ICoachModel CoachModel;

        //    if (this.IsConnectedToRemote == true)
        //    {
        //        ICoachPacket CoachPacket = await this.RemoteDBContext.GetEquipmentComponent_forWorkOrder(WONum);
        //        CoachModel = CoachPacket.CoachModels[0];
        //    }

        //    return null;
        //}

        //public async Task<List<ICoachModel>> GetElementAttibutes_forComponent(Component Component)
        //{
        //    ICoachModel CoachModel;

        //    if (this.IsConnectedToRemote == true)
        //    {
        //        ICoachPacket CoachPacket = await this.RemoteDBContext.GetElementAttribute_forComponent(Component);
        //        CoachModel = CoachPacket.CoachModels[0];
        //    }

        //    return null;
        //}
    }
}
