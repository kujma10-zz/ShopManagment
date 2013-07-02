using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopManagment.Controllers
{
    public class StoragesController : Controller
    {
        private ShopEntities db = new ShopEntities();

        //
        // GET: /Storages/

        public ActionResult Index()
        {
            return View(db.Storages.ToList());
        }

        //
        // GET: /Storages/Details/5

        public ActionResult Details(int id = 0)
        {
            Storage storage = db.Storages.Find(id);
            if (storage == null)
            {
                return HttpNotFound();
            }
            return View(storage);
        }

        //
        // GET: /Storages/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Storages/Create

        [HttpPost]
        public ActionResult Create(Storage storage)
        {
            if (ModelState.IsValid)
            {
                db.Storages.Add(storage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(storage);
        }

        //
        // GET: /Storages/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Storage storage = db.Storages.Find(id);
            if (storage == null)
            {
                return HttpNotFound();
            }
            return View(storage);
        }

        //
        // POST: /Storages/Edit/5

        [HttpPost]
        public ActionResult Edit(Storage storage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storage);
        }

        //
        // GET: /Storages/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Storage storage = db.Storages.Find(id);
            if (storage == null)
            {
                return HttpNotFound();
            }
            return View(storage);
        }

        //
        // POST: /Storages/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Storage storage = db.Storages.Find(id);
            db.Storages.Remove(storage);
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