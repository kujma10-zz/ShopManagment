using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopManagment.Controllers
{
    public class ShopController : Controller
    {
        //
        // GET: /Shop/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sales()
        {
            return View();
        }

        public ActionResult History()
        {
            return View();
        }
    }
}
