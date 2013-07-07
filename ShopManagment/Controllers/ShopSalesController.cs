using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;

namespace ShopManagment.Controllers
{
    public class ShopSalesController : Controller
    {
        private ShopEntities db = new ShopEntities();

        //
        // GET: /ShopSales/

        public ActionResult Index()
        {
            DateTime today = Convert.ToDateTime(DateTime.Today).Date;
            return View(db.Sales.Where(s => s.Returned != true &&
                        EntityFunctions.TruncateTime(s.Date) == today).ToList());
        }

        //
        // GET: /ShopSales/Details/5

        public ActionResult Details(int id = 0)
        {
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        //
        // GET: /ShopSales/Create

        public ActionResult Create()
        {
            ViewBag.AdminID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.CatID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            ViewBag.StorageID = new SelectList(db.Storages, "ID", "Name");
            return View();
        }

        //
        // POST: /ShopSales/Create

        [HttpPost]
        public ActionResult Create(Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Sales.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminID = new SelectList(db.Admins, "ID", "Username", sale.AdminID);
            ViewBag.CatID = new SelectList(db.Categories, "ID", "Name", sale.CatID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", sale.ProductID);
            ViewBag.StorageID = new SelectList(db.Storages, "ID", "Name", sale.StorageID);
            return View(sale);
        }

        //
        // GET: /ShopSales/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminID = new SelectList(db.Admins, "ID", "Username", sale.AdminID);
            ViewBag.CatID = new SelectList(db.Categories, "ID", "Name", sale.CatID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", sale.ProductID);
            ViewBag.StorageID = new SelectList(db.Storages, "ID", "Name", sale.StorageID);
            return View(sale);
        }

        //
        // POST: /ShopSales/Edit/5

        [HttpPost]
        public ActionResult Edit(Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminID = new SelectList(db.Admins, "ID", "Username", sale.AdminID);
            ViewBag.CatID = new SelectList(db.Categories, "ID", "Name", sale.CatID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", sale.ProductID);
            ViewBag.StorageID = new SelectList(db.Storages, "ID", "Name", sale.StorageID);
            return View(sale);
        }

        //
        // GET: /ShopSales/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        //
        // POST: /ShopSales/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sales.Find(id);
            db.Sales.Remove(sale);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}