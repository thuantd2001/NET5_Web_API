using MyWebApiApp.Models;
using System.Collections.Generic;

namespace MyWebApiApp.Services
{
    public interface IProductRepository
    {
        List<ProductModel> getAll(string search, double? from, double? to, string sortBy, int page = 1);
        //ProductVM createProduct(ProductModel product);
    }
}
