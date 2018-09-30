using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_LINQ_ClassOfProducts
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }

        public string Units { get; set; }

        public string Price { get; set; }

        public string CanBringOnBus { get; set; }
        public object Unit { get; internal set; }
        public bool UnitCanBringOnBus { get; internal set; }
    } 
}
