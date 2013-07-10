using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopManagment.Controllers
{
    [Authorize(Roles = ("StorageOperator"))]
    public class ProductsController : Controller
    {
        private ShopEntities db = new ShopEntities();

        //
        // GET: /Products/

        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        //
        // GET: /Products/Details/5

        public ActionResult Details(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Products/Create

        public ActionResult Create()
        {
            ViewBag.CatID = new SelectList(db.Categories.Where(a => a.Disabled == false), "ID", "Name");
            return View();
        }

        //
        // POST: /Products/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                bool exists = db.Products.Where(a => a.Name.Equals(product.Name)).ToList().Count()!=0;
                if (!exists)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "პროდუქტი ამ სახელით უკვე არსებობს!");
            }

            ViewBag.CatID = new SelectList(db.Categories.Where(a=>a.Disabled == false), "ID", "Name", product.CatID);
            return View(product);
        }

        //
        // GET: /Products/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatID = new SelectList(db.Categories.Where(a => a.Disabled == false), "ID", "Name", product.CatID);
            return View(product);
        }

        //
        // POST: /Products/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                bool exists = db.Products.Where(a => a.Name.Equals(product.Name) && a.ID != product.ID).ToList().Count()!=0;
                if (!exists)
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "პროდუქტი ამ სახელით უკვე არსებობს!");
            }
            ViewBag.CatID = new SelectList(db.Categories.Where(a => a.Disabled == false), "ID", "Name", product.CatID);
            return View(product);
        }

        //
        // GET: /Products/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Products/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            if (db.Balances.Any(a => a.ProductID == id))
            {
                ModelState.AddModelError("", "პროდუქტის ტიპის გაუქმებულია შეუძლებელია. ამ ტიპზე საწყობში რეგისტრირებულია პროდუქცია.");
                return View(product);
            }
            else
            {
                product.Disabled = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}