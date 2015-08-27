using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Client
{
    class Program
    {
        private static HttpClient client = new HttpClient();
        private static int workId;

        static void Main(string[] args)
        {
            client.BaseAddress = new Uri("http://localhost:15300");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Try get all products");
            ListAllProducts();
            Console.WriteLine("---------------------------------------");

            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Try add new product");
            AddNewProduct();
            Console.WriteLine("---------------------------------------");

            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Try update product");
            UpdateProduct();
            Console.WriteLine("---------------------------------------");

            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Try remove product");
            RemoveProduct();
            Console.WriteLine("---------------------------------------");
            ListAllProducts();
            Console.WriteLine("---------------------------------------");

            Console.WriteLine("Press Enter to quit.");
            Console.ReadKey();
        }

        private static void RemoveProduct()
        {
            var resp = client.DeleteAsync(string.Format("api/products/{0}", workId)).Result;
            resp.EnsureSuccessStatusCode();

            Console.WriteLine("Delete complite");
            Console.WriteLine("ID: {0}", workId);
        }

        private static void UpdateProduct()
        {
            var resp = client.GetAsync(string.Format("api/products/{0}", workId)).Result;
            resp.EnsureSuccessStatusCode();

            var workProduct = resp.Content.ReadAsAsync<Product>().Result;

            workProduct.Category = "КатегорияСуперПупер";
            workProduct.Name = "МегаСуперПродукт";
            workProduct.Price = 331.03m;

            var resp2 = client.PutAsJsonAsync(string.Format("api/products/{0}", workId), workProduct).Result;
            resp2.EnsureSuccessStatusCode();

            resp = client.GetAsync(string.Format("api/products/{0}", workId)).Result;
            resp.EnsureSuccessStatusCode();

            var updatedProduct = resp.Content.ReadAsAsync<Product>().Result;
            Console.WriteLine("Update complite");
            Console.WriteLine("{0} {1} {2:c} ({3})", updatedProduct.Id, updatedProduct.Name, updatedProduct.Price, updatedProduct.Category);
        }

        static void ListAllProducts()
        {
            var resp = client.GetAsync("api/products").Result;
            resp.EnsureSuccessStatusCode();

            var products = resp.Content.ReadAsAsync<IEnumerable<Product>>().Result;
            foreach (var p in products)
            {
                Console.WriteLine("{0} {1} {2:c} ({3})", p.Id, p.Name, p.Price, p.Category);
            }
        }

        static void AddNewProduct()
        {
            var product = new Product {Category = "КатегорияСупер", Name = "МегаПродукт", Price = 231.03m};
            var resp = client.PostAsJsonAsync("api/products", product).Result;
            resp.EnsureSuccessStatusCode();

            var addProduct = resp.Content.ReadAsAsync<Product>().Result;
            Console.WriteLine("Insert complite");
            Console.WriteLine("{0} {1} {2:c} ({3})", addProduct.Id, addProduct.Name, addProduct.Price, addProduct.Category);
            workId = addProduct.Id;
        }
    }


    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}
