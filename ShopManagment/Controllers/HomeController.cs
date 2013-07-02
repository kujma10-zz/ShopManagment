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
        //private ShopEntities db = new ShopEntities();
        /*
         *   <li><span>@Html.ActionLink("კატეგორიები", "Index", "Categories")</span></li>
             <li><span>@Html.ActionLink("პროდუქტები", "Index", "Products")</span></li>
             <li><<span>@Html.ActionLink("საწყობების მართვა", "Index", "Storages")</span></li>
         * 
         * */
        //
        // GET: /Categories/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
