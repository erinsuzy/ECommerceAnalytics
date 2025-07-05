using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAnalytics
{
    public class Order
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }

        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public string Country { get; set; }
        public string City { get; set; }


        public Order()
        {
            OrderId = string.Empty;
            Country = string.Empty;
            City = string.Empty;

        }
    }
}
