using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopManagment.Models
{
    public class History
    {
        public IEnumerable<Sale> sale { get; set; }
        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
       // [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [Required(ErrorMessage = "Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
      //  [DataType(DataType.Date)]
        [Display(Name = "To")]
        public DateTime ToDate { get; set; }
        public int AdminID { get; set; }
    }
}