using CsvHelper;
using CsvHelper.Configuration;
using ECommerceAnalytics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ECommerceAnalytics
{
    public class CsvGenerator
    {
        private static readonly Random rand = new Random();
        private static readonly List<Product> products = new List<Product>()
        {
            new() { ProductId = 1001, Name = "Wireless Mouse", BasePrice = 19.99m },
            new() { ProductId = 1002, Name = "Bluetooth Headphones", BasePrice = 49.99m },
            new() { ProductId = 1003, Name = "Mechanical Keyboard", BasePrice = 89.99m },
            new() { ProductId = 1004, Name = "USB-C Cable", BasePrice = 9.99m },
            new() { ProductId = 1005, Name = "Webcam", BasePrice = 39.99m },
            new() { ProductId = 1006, Name = "Laptop Stand", BasePrice = 25.99m },
            new() { ProductId = 1007, Name = "4K Monitor", BasePrice = 229.99m },
            new() { ProductId = 1008, Name = "Portable SSD", BasePrice = 79.99m },
            new() { ProductId = 1009, Name = "Smartphone Case", BasePrice = 15.99m },
            new() { ProductId = 1010, Name = "Wireless Charger", BasePrice = 29.99m },

        };

        private static readonly Dictionary<string, List<string>> citiesByCountry = new()
        {
            { "USA", new() { "New York", "Los Angeles", "Chicago", "Houston", "Seattle" }
            },
            { "Canada", new() { "Toronto", "Vancouver", "Montreal", "Ottowa" }
            },
            { "UK", new() { "London", "Manchester", "Bristol", "Birmingham" }
            },
            { "Germany", new() { "Berlin", "Munich", "Hamburg", "Frankfurt" }
            },
            { "France", new() { "Paris", "Lyon", "Marseille", "Toulouse" }
            },
            { "Australia", new() { "Sydney", "Melbourne", "Brisbane", "Perth" }
            },
            { "Japan", new() { "Tokyo", "Osaka", "Yokohama", "Nagoya" }
            },
            { "Brazil", new() { "Sao Paulo", "Rio de Janeiro", "Brasilia", "Salvador" }
            },
            { "India", new() { "Mumbai", "Dehli", "Bangalore", "Hyderabad" }
            },
            { "Mexico", new() { "Mexico City", "Guadalajara", "Monterrey", "Puebla" }
            },

        };
        private static readonly List<string> countries = new(citiesByCountry.Keys);

        public static void GenerateCsv(string path, int numberOfRecords)
        {
            using var writer = new StreamWriter(path, false, Encoding.UTF8);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            csv.Context.RegisterClassMap<OrderMap>();
            csv.WriteHeader<Order>();
            csv.NextRecord();

            DateTime now = DateTime.UtcNow;
            DateTime startDate = now.AddDays(-365);

            for (int i = 0; i < numberOfRecords; i++)
            {
                var orderId = Guid.NewGuid().ToString();

                int randomDays = rand.Next(365);
                DateTime orderDate = startDate.AddDays(randomDays);

                int userId = rand.Next(100000, 1000000);
                var product = products[rand.Next(products.Count)];
                int quantity = rand.Next(1, 6);
                decimal price = Math.Round(product.BasePrice * (decimal)(0.9 + rand.NextDouble() * 0.2), 2);
                decimal totalAmount = Math.Round(price * quantity, 2);

                string country = countries[rand.Next(countries.Count)];
                string city = citiesByCountry[country][rand.Next(citiesByCountry[country].Count)];

                var order = new Order
                {
                    OrderId = orderId,
                    OrderDate = orderDate,
                    UserId = userId,
                    ProductId = product.ProductId,
                    Quantity = quantity,
                    Price = price,
                    TotalAmount = totalAmount,
                    Country = country,
                    City = city,
                };

                csv.WriteRecord(order);
                csv.NextRecord();

                //Console progress

                if ((i + 1) % 10000 == 0)
                    Console.WriteLine($"{i + 1} records written...");

            }
            Console.WriteLine($"Successfully wrote {numberOfRecords : N0} records to {path}");
        }

 
    }
}

public sealed class OrderMap : ClassMap<Order>
{
    public OrderMap()
    {
        Map(m => m.OrderId).Name("order_id");
        Map(m => m.OrderDate).Name("order_date");
        Map(m => m.UserId).Name("user_id");
        Map(m => m.ProductId).Name("product_id");
        Map(m => m.Quantity).Name("quantity");
        Map(m => m.Price).Name("price");
        Map(m => m.TotalAmount).Name("total_amount");
        Map(m => m.Country).Name("country");
        Map(m => m.City).Name("city");
    }
}