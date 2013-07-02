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

        //
        // GET: /Storages/Move/5

        public ActionResult Move()
        {
            ViewBag.StorageID = new SelectList(db.Storages, "ID", "Name");
            ViewBag.CatID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            return View();
        }

        //
        // POST: /Storages/Move/5

        [HttpPost]
        public ActionResult Move(MoveProducts mp)
        {
            if (mp.StorageFromID == mp.StorageToID)
            {
                ModelState.AddModelError("", "აირჩიეთ სხვადასხვა საწყობები" + mp.Quantity);
            }
            else
            {
                Balance from = db.Balances.Find(mp.StorageFromID, mp.CatID, mp.ProductID);
                Balance to = db.Balances.Find(mp.StorageToID, mp.CatID, mp.ProductID);
                if (from == null)
                {
                    ModelState.AddModelError("", "ასეთი პროდუქტი არ არსებობს საწყობში");
                }
                else if (to == null)
                {
                    ModelState.AddModelError("", "ასეთი პროდუქტი არ არსებობს მეორე საწყობში");
                }
                else
                {
                    if (mp.Quantity > from.Quantity)
                    {
                        ModelState.AddModelError("", "მითითებული პროდუქციის რაოდენობა აღემატება საწყობის ნაშთს");
                    }
                    else
                    {
                        from.Quantity = from.Quantity - mp.Quantity;
                        to.Quantity = to.Quantity + mp.Quantity;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    
                }
                
            }
            ViewBag.StorageID = new SelectList(db.Storages, "ID", "Name");
            ViewBag.CatID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}