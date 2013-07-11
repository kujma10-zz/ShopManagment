using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;
using System.Data.Linq;

namespace ShopManagment.Controllers
{
    [Authorize(Roles = ("ShopOperator"))]
    public class ShopSalesController : Controller
    {
        private ShopEntities db = new ShopEntities();

        //
        // GET: /ShopSales/

        public ActionResult Index()
        {
            DateTime today = Convert.ToDateTime(DateTime.Today).Date;
            var adminInfo = db.Admins.Where(a => a.Username.Equals(User.Identity.Name)).First();
            return View(db.Sales.Where(s => EntityFunctions.TruncateTime(s.Date) == today && s.AdminID==adminInfo.ID).ToList());
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
            var adminInfo = db.Admins.Where(a => a.Username.Equals(User.Identity.Name)).First();
            sale.AdminID = adminInfo.ID;
            sale.Date = DateTime.Now;
            sale.Returned = false;
            if (ModelState.IsValid)
            {
                affectStorage(sale);
                db.Sales.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatID = new SelectList(db.Categories, "ID", "Name", sale.CatID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", sale.ProductID);
            ViewBag.StorageID = new SelectList(db.Storages, "ID", "Name", sale.StorageID);
            return View(sale);
        }

        private void affectStorage(Sale sale)
        {
            var a = db.Balances.Where(s=>s.StorageID == sale.StorageID && s.ProductID == sale.ProductID).First();
            Balance b = a;
            b.Quantity -= sale.Quantity;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
            }
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
            sale.Returned = true;
            returnToStorage(sale);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        private void returnToStorage(Sale sale)
        {
            var a = db.Balances.Where(s => s.StorageID == sale.StorageID && s.ProductID == sale.ProductID).First();
            Balance b = a;
            b.Quantity += sale.Quantity;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
            }
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

        public ActionResult GetListValue(String category, String product, String storage)
        {
            int categoryID = Convert.ToInt32(category);
            int productID = Convert.ToInt32(product);
            int storageID = Convert.ToInt32(storage);

            var quantityResult = db.Balances.Where(b => b.CatID == categoryID && b.ProductID == productID && b.StorageID == storageID);
            int quantity = 0;
            if (quantityResult.Count() > 0)
            {
                quantity = quantityResult.First().Quantity;
            }
            else
            {
                quantity = 0;
            }

            var priceResult = db.Products.Where(p => p.ID == productID && p.CatID==categoryID);
            double? price = 0;
            if (priceResult.Count() > 0)
            {
                price = priceResult.First().Price;
            }
            else
            {
                price = 0;
            }

            return Json(new { price = price, quantity = quantity }, JsonRequestBehavior.AllowGet);
        }
    }
}