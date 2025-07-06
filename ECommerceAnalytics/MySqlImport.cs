using CsvHelper;
using CsvHelper.Configuration;
using Google.Protobuf;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAnalytics
{
    public class MySqlImport
    {
        private readonly string _connectionString;

        public MySqlImport(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ImportCsv(string csvPath)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            csv.Context.RegisterClassMap<OrderMap>();

            var orders = csv.GetRecords<Order>();

            foreach (var order in orders)
            {
                string query = @"
                    INSERT INTO orders (
                    order_id, order_date, user_id, product_id, quantity, price, total_amount, country, city
                    )

                    VALUES (
                    @order_id, @order_date, @user_id, @product_id, @quantity, @price, @total_amount, @country, @city
                    );";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@order_id", order.OrderId);
                cmd.Parameters.AddWithValue("@order_date", order.OrderDate);
                cmd.Parameters.AddWithValue("@user_id", order.UserId);
                cmd.Parameters.AddWithValue("@quantity", order.Quantity);
                cmd.Parameters.AddWithValue("@price", order.Price);
                cmd.Parameters.AddWithValue("@total_amount", order.TotalAmount);
                cmd.Parameters.AddWithValue("@country", order.Country);
                cmd.Parameters.AddWithValue("@city", order.City);

                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Import complete!");
        }


    }
}
