using ECommerceAnalytics;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Generating e-commerce data...");
        CsvGenerator.GenerateCsv("synthetic_orders.csv", 1_000_000);

        string connectionString = "server=localhost; port=3306; user=ecommerceanalytics; password=ecommerceanalytics; database=ecommerceanalytics";
        var importer = new MySqlImport(connectionString);

        importer.ImportCsv("synthetic_orders.csv");
    }
}