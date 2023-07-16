using DataManager.Library.DataAccess;
using DataManager.Library.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Http;

namespace DataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();

            data.SaveSale(sale, RequestContext.Principal.Identity.GetUserId());
        }
    }

}
