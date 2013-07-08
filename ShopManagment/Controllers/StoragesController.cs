﻿using ShopManagment.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopManagment.Controllers
{
    [AdminOnly(Role = AdminOnlyAttribute.AdminRole.StorageOperator)]
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
                bool exists = db.Storages.Where(a => a.Name.Equals(storage.Name)).ToList().Count()!=0;
                if (!exists)
                {
                    storage.Opened = DateTime.Now;
                    db.Storages.Add(storage);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "საწყობი ამ სახელით უკვე არსებობს!");
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
                bool exists = db.Storages.Where(a => a.Name.Equals(storage.Name) && a.ID != storage.ID).ToList().Count()!=0;
                if (!exists)
                {
                    
                    db.Entry(storage).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "საწყობი ამ სახელით უკვე არსებობს!");
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
            var items = db.Balances.Where(a => a.StorageID == id && a.Quantity > 0).ToList();
            if (items.Count() == 0)
            {
                storage.Closed = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "საწყობი არ არის ცარიელი, საწყობის დახურვისათვის აუცილებელია საწყობის გაცარიელება, ან პროდუქციის სხვა საწყობებში გადანაწილება");
            return View(storage);

        }

        //
        // GET: /Storages/Move/5

        public ActionResult Move()
        {
            ViewBag.StorageFromID = new SelectList(db.Storages.Where(a => a.Closed == null), "ID", "Name");
            ViewBag.StorageToID = new SelectList(db.Storages.Where(a => a.Closed == null), "ID", "Name");
            ViewBag.CatID = new SelectList(db.Categories.Where(a => a.Disabled == false), "ID", "Name");
            ViewBag.ProductID = new SelectList(db.Products.Where(a => a.Disabled == false), "ID", "Name");
            return View();
        }

        //
        // POST: /Storages/Move/5

        [HttpPost]
        public ActionResult Move(MoveProducts mp)
        {
            if (ModelState.IsValid)
            {
                bool isOpen1 = db.Storages.First(a => a.ID == mp.StorageFromID).Closed == null;
                bool isOpen2 = db.Storages.First(a => a.ID == mp.StorageToID).Closed == null;
                if (!isOpen1 || !isOpen2)
                {
                    ModelState.AddModelError("", "დახურულ საწყობებზე შეუძლებელია ოპერაციის განხორციელება.");
                }
                else if (mp.StorageFromID == mp.StorageToID)
                {
                    ModelState.AddModelError("", "აირჩიეთ სხვადასხვა საწყობები.");
                }
                else
                {
                    var from = from a in db.Balances where a.StorageID == mp.StorageFromID && a.CatID == mp.CatID && a.ProductID == mp.ProductID select a;
                    var to = from a in db.Balances where a.StorageID == mp.StorageToID && a.CatID == mp.CatID && a.ProductID == mp.ProductID select a;

                    if (from.ToList().Count == 0)
                    {
                        ModelState.AddModelError("", "ასეთი პროდუქტი არ არსებობს საწყობში.");
                    }
                    else if (mp.Quantity > from.First().Quantity)
                    {
                        ModelState.AddModelError("", "მითითებული პროდუქციის რაოდენობა აღემატება საწყობის ნაშთს.");
                    }
                    else
                    {
                        if (to.ToList().Count == 0)
                        {
                            // ასეთი პროდუქტი დანიშნულების საწყობში არ არსებობს ჯერ და ამიტომ ახალს ვქმნით
                            Balance b = new Balance();
                            b.ProductID = mp.ProductID;
                            b.CatID = mp.CatID;
                            b.StorageID = mp.StorageToID;
                            b.Quantity = mp.Quantity;
                            db.Balances.Add(b);
                        }
                        else
                        {
                            to.First().Quantity = to.First().Quantity + mp.Quantity;
                        }
                        from.First().Quantity = from.First().Quantity - mp.Quantity;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }


                }
            }
            ViewBag.StorageFromID = new SelectList(db.Storages.Where(a => a.Closed == null), "ID", "Name");
            ViewBag.StorageToID = new SelectList(db.Storages.Where(a => a.Closed == null), "ID", "Name");
            ViewBag.CatID = new SelectList(db.Categories.Where(a => a.Disabled == false), "ID", "Name");
            ViewBag.ProductID = new SelectList(db.Products.Where(a => a.Disabled == false), "ID", "Name");
            return View();
        }

        //
        // GET: /Storages/Purchase/5

        public ActionResult Purchase(int id = 0)
        {
            Balance b = new Balance();
            b.StorageID = id;
            ViewBag.CatID = new SelectList(db.Categories.Where(a => a.Disabled == false), "ID", "Name");
            ViewBag.ProductID = new SelectList(db.Products.Where(a => a.Disabled == false), "ID", "Name");
            return View(b);
        }

        //
        // POST: /Storages/Purchase/5

        [HttpPost]
        public ActionResult Purchase(Balance balance)
        {
            if (ModelState.IsValid)
            {
                var item = db.Products.Where(a => a.CatID == balance.CatID && a.ID == balance.ProductID).ToList();
                if (item.Count() == 0)
                {
                    ModelState.AddModelError("", "კატეგორიას არ შეესაბამება პროდუქტი");
                    ViewBag.CatID = new SelectList(db.Categories.Where(a => a.Disabled == false), "ID", "Name");
                    ViewBag.ProductID = new SelectList(db.Products.Where(a => a.Disabled == false), "ID", "Name");
                    return View(balance);
                }
                var result = from a in db.Balances where a.StorageID == balance.StorageID && a.CatID == balance.CatID && a.ProductID == balance.ProductID select a;
                if (result.ToList().Count() == 0)
                {
                    db.Balances.Add(balance);
                }
                else
                {
                    Balance b = result.First();
                    b.Quantity = b.Quantity + balance.Quantity;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatID = new SelectList(db.Categories.Where(a => a.Disabled == false), "ID", "Name");
            ViewBag.ProductID = new SelectList(db.Products.Where(a => a.Disabled == false), "ID", "Name");
            return View(balance);

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}