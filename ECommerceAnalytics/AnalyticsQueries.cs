using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAnalytics
{
    public class AnalyticsQueries
    {
        public static string GetTotalSalesPerDayQuery()
        {
            return @"
            SELECT order_date, SUM(total_amount) AS total_sales
            FROM orders 
            GROUP BY order_date
            ORDER BY order_date;
            ";
        }

        public static string GetTopSellingProductsQuery()
        {
            return @"
            SELECT 
                p.name as product_name, 
                SUM (o.quantity) AS total_sold
            FROM orders o
            JOIN products p on p.product_id = o.product_id
            GROUP BY p.name
            ORDER BY total_sold DESC
            LIMIT 10;
            ";

        }

        public static string GetSalesByDateRangeQuery()
        {
            return @"
            SELECT order_date, SUM(total_amount) as total_sales
            FROM orders
            WHERE order_date BETWEEN @startDate and @endDate
            GROUP BY order_date
            ORDER BY order_date;
            ";
        }

        public static string GetTotalSalesByCountryQuery()
        {
            return @"
            SELECT country, SUM(total_amount) as total_sales
            FROM orders
            GROUP BY country
            ORDER BY total_sales DESC
            LIMIT 10;
            ";
        }

        public static string GetTopUsersBySpendQuery()
        {
            return @"
            SELECT u.user_id, u.country, u.city, SUM(o.total_amount) as total_spent
            FROM orders o 
            JOIN users u on u.user_id = o.user_id
            GROUP BY u.user_id, u.country, u.city
            ORDER BY total_spent DESC
            LIMIT 10; 
            ";
        }
    }
}