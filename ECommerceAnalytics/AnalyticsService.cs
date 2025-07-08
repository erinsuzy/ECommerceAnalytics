using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;

namespace ECommerceAnalytics
{
    public class AnalyticsService
    {

        private readonly string _connectionString;

        public AnalyticsService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<(DateTime Date, decimal TotalSales)> GetSalesByDateRange(DateTime startDate, DateTime endDate)
        {
            var results = new List<(DateTime, decimal)>();
            string query = AnalyticsQueries.GetSalesByDateRangeQuery();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var date = reader.GetDateTime("order_date");
                var total = reader.GetDecimal("total_sales");
                results.Add((date, total));
            }
            return results;
        }

        public List<DailySalesResult> GetTotalSalesPerDay()
        {
            var query = AnalyticsQueries.GetTotalSalesPerDayQuery();
            var results = new List<DailySalesResult>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                results.Add(new DailySalesResult
                {
                    OrderDate = reader.GetDateTime("order_date"),
                    TotalSales = reader.GetDecimal("total_sales")
                });

            }
            return results;
        }

        public List<string> GetTopSellingProducts()
        {
            var query = AnalyticsQueries.GetTopSellingProductsQuery();
            var results = new List<string>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string productName = reader.GetString("product_name");
                int totalSold = reader.GetInt32("total_sold");

                results.Add(@"{productName} - Sold: {totalSold}");

            }
            return results;
        }

        public List<string> GetTotalSalesByCountry()
        {
            var query = AnalyticsQueries.GetTotalSalesByCountryQuery();
            var results = new List<string>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string countryName = reader.GetString("country");
                decimal totalSales = reader.GetDecimal("total_sales");

                results.Add($"{countryName} sold {totalSales}");
            }
            return results;
        }

        public List<string> GetTopSpendersBySpend()
        {
            var query = AnalyticsQueries.GetTopUsersBySpendQuery();
            var results = new List<string>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int userId = reader.GetInt32("user_id");
                string userCountry = reader.GetString("country");
                string userCity = reader.GetString("city");
                decimal totalSales = reader.GetDecimal("total_sales");

                results.Add($"User ID: {userId}, Country: {userCountry}, City: {userCity} Total Sales: ${totalSales:N2}");
            }
            return results;
        }
    }
}
