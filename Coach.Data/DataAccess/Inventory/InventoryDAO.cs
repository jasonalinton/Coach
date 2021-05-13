using Coach.Data.Model;
using Coach.Model.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Coach.Data.Mappping.MyMapper;

namespace Coach.Data.DataAccess.Inventory
{
    public interface IInventoryDAO
    {
        List<InventoryItemModel> GetInventoryItems();
    }

    public class InventoryDAO : IInventoryDAO
    {
        public List<InventoryItemModel> GetInventoryItems()
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var inventoryItems = entities.inventoryitems.ToList();
                return CoachMapper.Map<List<InventoryItemModel>>(inventoryItems);
            }
        }
    }
}
