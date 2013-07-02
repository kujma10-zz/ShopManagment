using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopManagment.Models
{
    public class MoveProducts
    {
        public int StorageFromID;
        public int StorageToID;
        public int CatID;
        public int ProductID;
        public int Quantity;
    }
}