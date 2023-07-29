using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RMApi.Models;
using System.Diagnostics;

namespace RMApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private RoleManager<IdentityRole> _roleManager;
        //private UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger)
            //RoleManager<IdentityRole> roleManager,
            //UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            //_roleManager = roleManager;
            //_userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Privacy()
        public IActionResult Privacy()
        {
            //string[] roles = { "Admin", "Manager", "Cashier" };

            //foreach (var role in roles)
            //{
            //    var roleExist = await _roleManager.RoleExistsAsync(role);

            //    if (roleExist == false)
            //    {
            //        await _roleManager.CreateAsync(new IdentityRole(role));
            //    }
            //}

            //var user = await _userManager.FindByEmailAsync("linoy@gmail.com");

            //if (user != null)
            //{
            //    await _userManager.AddToRoleAsync(user, "Admin");
            //    await _userManager.AddToRoleAsync(user, "Cashier");
            //}

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}