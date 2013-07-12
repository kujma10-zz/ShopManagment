﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopManagment.Views.ShopHistoryManag
{
    public class ShopHistoryManagController : Controller
    {
        private ShopEntities db = new ShopEntities();


        //
        // GET: /ShopHistory/

        public ActionResult Index()
        {
            Models.History history = new Models.History();
            history.FromDate = DateTime.Now.AddDays(-3);
            history.ToDate = DateTime.Now;
            history.sale = db.Sales.Where(a => a.Date >= history.FromDate && a.Date <= history.ToDate).Include(s => s.Admin).Include(s => s.Category).Include(s => s.Product).Include(s => s.Storage);

/*
            List<SelectListItem> items = new List<SelectListItem>();
            var query = db.webpages_UsersInRoles.Select(c => new { c.UserID, c.RoleID });
            foreach (var a in query)
            {
                if (a.RoleID == 2)   // Shop Operator - is RoleID = 2 s ?
                {
                    var b = (from roles in db.Admins where roles.ID == a.UserID select roles).toList();
                    SelectListItem listItem = new SelectListItem { Value = b.ID.ToString(), Text = b.Username };
                    items.Add(listItem);
                }
            }

            ViewBag.AdminID = new SelectList(items, "Value", "Text");
       */
            ViewBag.AdminID = new SelectList(db.Admins, "ID", "Username");
            return View(history);
        }

        //
        // POST: /ShopHistory/

        [HttpPost]
        public ActionResult Index(Models.History history)
        {
            if (history.AdminID == 0)
            {
                history.sale = db.Sales.Where(a => a.Date >= history.FromDate && a.Date <= history.ToDate).Include(s => s.Admin).Include(s => s.Category).Include(s => s.Product).Include(s => s.Storage);
            }
            else
            {
                history.sale = db.Sales.Where(a => a.Date >= history.FromDate && a.Date <= history.ToDate && a.AdminID == history.AdminID).Include(s => s.Admin).Include(s => s.Category).Include(s => s.Product).Include(s => s.Storage);
            }

       /*
            List<SelectListItem> items = new List<SelectListItem>();
            var query = db.webpages_UsersInRoles.Select(c => new { c.UserID, c.RoleID });
            foreach (var a in query)
            {
                if (a.RoleID == 2)   // Shop Operator - is RoleID = 2 s ?
                {
                    var b = (from roles in db.Admins where roles.ID == a.UserID select roles).toList();
                    SelectListItem listItem = new SelectListItem { Value = b.ID.ToString(), Text = b.Username };
                    items.Add(listItem);
                }
            }

            ViewBag.AdminID = new SelectList(items, "Value", "Text");
       */


            ViewBag.AdminID = new SelectList(db.Admins, "ID", "Username");

            return View(history);
        }

        //
        // GET: /ShopHistory/Details/5

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
        // GET: /ShopHistory/Create

        public ActionResult Create()
        {
            ViewBag.AdminID = new SelectList(db.Admins, "ID", "Username");
            ViewBag.CatID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            ViewBag.StorageID = new SelectList(db.Storages, "ID", "Name");
            return View();
        }

        //
        // POST: /ShopHistory/Create

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
        // GET: /ShopHistory/Edit/5

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
        // POST: /ShopHistory/Edit/5

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
        // GET: /ShopHistory/Delete/5

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
        // POST: /ShopHistory/Delete/5

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
