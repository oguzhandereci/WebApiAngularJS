using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiAngularJS.Models.ViewModels
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string CategoryName { get; set; } = String.Empty;
    }
}