using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopManagment.Models
{
    public class MoveProducts
    {
        public int StorageFromID { get; set; }
        public int StorageToID { get; set; }
        public int CatID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}