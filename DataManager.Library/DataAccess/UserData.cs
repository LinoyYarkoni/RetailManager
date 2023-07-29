using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DataManager.Library.DataAccess
{
    public class UserData
    {
        private IConfiguration _config;

        public UserData(IConfiguration config)
        {
            _config = config;
        }

        public List<UserModel> GetUserById(string id)
        {
            SqlDataAccess sql = new SqlDataAccess(_config);
            var parameters = new { Id = id };

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUser_Lookup", parameters, "RMData");

            return output;
        }
    }
}
