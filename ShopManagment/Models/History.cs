using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopManagment.Models
{
    public class History
    {
        public IEnumerable<Sale> sale { get; set; }
        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [Required(ErrorMessage = "Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "To")]
        public DateTime ToDate { get; set; }

    }
}