using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain;

namespace Host
{
    public class ProductsController : ApiController
    {
        private static List<Product> _products = new List<Product>
        {
            new Product{ Id = 1, Category = "Категория1", Name = "Продукт1Кат1", Price = 125.00m},
            new Product{ Id = 2, Category = "Категория1", Name = "Продукт2Кат1", Price = 124.40m},
            new Product{ Id = 3, Category = "Категория2", Name = "Продукт3Кат2", Price = 1254.02m},
            new Product{ Id = 4, Category = "Категория3", Name = "Продукт4Кат3", Price = 4345.12m},
        };


        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }


        public Product GetProductById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return product;
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        public HttpResponseMessage PostProduct(Product product)
        {
            product.Id = _products.Count + 1;
            _products.Add(product);
            var response = Request.CreateResponse(HttpStatusCode.Created, product);
            var uri = Url.Link("API Default", new {id = product.Id});
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public void PutProduct(int id, Product product)
        {
            var index = _products.FindIndex(p => p.Id == id);
            if (index == -1)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            _products.RemoveAt(index);
            _products.Add(product);
        }

        public void DeleteProduct(int id)
        {
            var index = _products.FindIndex(p => p.Id == id);
            if (index == -1)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            _products.RemoveAt(index);
        }
    }
}
