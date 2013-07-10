using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopManagment.Controllers
{
    [Authorize(Roles = ("StorageOperator, ShopManager"))]
    public class BalancesController : Controller
    {

        private static ShopEntities entity = new ShopEntities();

        //
        // GET: /Balances/

        public ActionResult Index()
        {
            var loc = getTree();
       
            return View(loc);
        }

        public static List<TreeViewLocation> getTree()
        {
            var loc = new List<TreeViewLocation>();

            HashSet<TreeViewLocation> prods = new HashSet<TreeViewLocation>();
            HashSet<TreeViewLocation> categs = new HashSet<TreeViewLocation>();

            var stor = entity.Storages.ToList();

            foreach (var a in stor)
            {
                var categories = entity.Categories.ToList();
                categs = new HashSet<TreeViewLocation>();

                foreach (var c in categories)
                {
                    var bal = (from table in entity.Balances where table.StorageID == a.ID && table.CatID == c.ID select table).ToList();
                    prods = new HashSet<TreeViewLocation>();
                    foreach (var p in bal)
                    {
                        var productName = (from prs in entity.Products where prs.ID == p.ProductID select prs).ToList();
                        TreeViewLocation child = new TreeViewLocation { Name = (productName[0].Name + " " + p.Quantity) };
                        prods.Add(child);
                    }
                    if (prods.Count != 0)
                    {
                        TreeViewLocation child2 = new TreeViewLocation { Name = c.Name, ChildLocations = prods };
                        categs.Add(child2);
                    }
                }

                TreeViewLocation newLoc = new TreeViewLocation { Name = a.Name, ChildLocations = categs };
                loc.Add(newLoc);
            }

            return loc;
        }
    }

    public class TreeViewLocation
    {
        public TreeViewLocation()
        {
            ChildLocations = new HashSet<TreeViewLocation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TreeViewLocation> ChildLocations { get; set; }
    }

}
