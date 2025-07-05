using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAnalytics
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }


        public Product()
        {
            Name = string.Empty;
        }
    }
}
