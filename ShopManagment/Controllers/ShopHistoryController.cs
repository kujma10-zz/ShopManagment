using ShopManagment.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopManagment.Controllers
{
    [Authorize(Roles = ("ShopOperator, ShopManager"))]
    public class ShopHistoryController : Controller
    {
        private ShopEntities db = new ShopEntities();


        //
        // GET: /ShopHistory/

        public ActionResult Index()
        {
            History history = new History();
            history.FromDate = DateTime.Now.AddDays(-3);
            history.ToDate = DateTime.Now.Date;
            if (User.IsInRole("ShopManager"))
            {
                ViewBag.AdminID = new SelectList(db.Admins.Join(db.webpages_UsersInRoles, a => a.ID, b => b.UserId, (a, b) => new { a.ID, a.Username, b.RoleId }).Where(p => p.RoleId == 2), "ID", "Username");
                history.sale = db.Sales.Where(a => a.Date >= history.FromDate && a.Date <= history.ToDate).Include(s => s.Admin).Include(s => s.Category).Include(s => s.Product).Include(s => s.Storage);
            }
            else
            {
                int UserID = GetUserID();
                history.sale = db.Sales.Where(a => a.Date >= history.FromDate && a.Date <= history.ToDate && a.AdminID == UserID).Include(s => s.Admin).Include(s => s.Category).Include(s => s.Product).Include(s => s.Storage);
            }

            return View(history);
        }

        //
        // POST: /ShopHistory/

        [HttpPost]
        public ActionResult Index(History history)
        {
            if (User.IsInRole("ShopManager"))
            {
                if (history.AdminID == 0)
                {
                    history.sale = db.Sales.Where(a => a.Date >= history.FromDate && a.Date <= history.ToDate).Include(s => s.Admin).Include(s => s.Category).Include(s => s.Product).Include(s => s.Storage);
                }
                else
                {
                    history.sale = db.Sales.Where(a => a.Date >= history.FromDate && a.Date <= history.ToDate && a.AdminID == history.AdminID).Include(s => s.Admin).Include(s => s.Category).Include(s => s.Product).Include(s => s.Storage);
                }
                ViewBag.AdminID = new SelectList(db.Admins.Join(db.webpages_UsersInRoles, a => a.ID, b => b.UserId, (a, b) => new { a.ID, a.Username, b.RoleId }).Where(p => p.RoleId == 2), "ID", "Username");

            }
            else
            {
                int UserID = GetUserID();
                history.sale = db.Sales.Where(a => a.Date >= history.FromDate && a.Date <= history.ToDate && a.AdminID == UserID).Include(s => s.Admin).Include(s => s.Category).Include(s => s.Product).Include(s => s.Storage);
            }

            return View(history);

        }

        private int GetUserID()
        {
            var adminInfo = db.Admins.Where(a => a.Username.Equals(User.Identity.Name)).First();
            return adminInfo.ID;
        }




    }
}