using DataManager.Library.DataAccess;
using DataManager.Library.Models;
using DataManager.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        List<ApplicationUserModel> output = new List<ApplicationUserModel>();

        [HttpGet]
        public UserModel GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();

            return data.GetUserById(userId).First();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            using(var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                foreach (var user in users)
                {
                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                    };

                    foreach (var role in user.Roles)
                    {
                        u.Roles.Add(role.RoleId, roles.Where(x => x.Id == role.RoleId).First().Name);
                    }

                    output.Add(u);
                }
            }

            return output;
        }
    }
}
