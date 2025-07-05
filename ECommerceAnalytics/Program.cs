using ECommerceAnalytics;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Generating e-commerce data...");
        CsvGenerator.GenerateCsv("synthetic_orders.csv", 1_000_000);
    }
}