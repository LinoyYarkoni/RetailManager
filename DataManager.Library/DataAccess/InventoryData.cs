using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using System.Collections.Generic;

namespace DataManager.Library.DataAccess
{
    public class InventoryData
    {
        public List<InventoryModel> GetInventory()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "RMData");

            return output;
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            SqlDataAccess sql = new SqlDataAccess();

            sql.SaveData<InventoryModel>("dbo.spInventory_Insert", item, "RMData");
        }
    }
}
