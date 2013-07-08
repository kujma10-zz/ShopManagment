using ShopManagment.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace ShopManagment.Controllers
{

    public class HomeController : Controller
    {
   

        public ActionResult Index()
        {
            return View();
        }

    }
}
