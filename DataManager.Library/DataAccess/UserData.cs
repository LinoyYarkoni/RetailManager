using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using System.Collections.Generic;

namespace DataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var parameters = new { Id = id };

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUser_Lookup", parameters, "RMData");

            return output;
        }
    }
}
