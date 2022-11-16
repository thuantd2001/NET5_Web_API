using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using MyWebApiApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApiApp.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _context;
        public static int PAGE_SIZE { get; set; } = 5;
        public ProductRepository(MyDbContext context)
        {
            _context = context;
        }

        //public ProductVM createProduct(ProductModel product)
        //{
        //    var _product = new Product();
        //    //var _type = new Type
        //    //{
        //    //    NameType = type.Name,
        //    //};
        //    //_context.Add(_type);
        //    //_context.SaveChanges();
        //    //return new TypeVM
        //    //{
        //    //    IdType = _type.IdType,
        //    //    NameType = _type.NameType,
        //    //};
        //}

        public List<ProductModel> getAll(string search, double? from, double? to, string sortBy, int page = 1)
        {
            var allProducts = _context.Products.Include(p => p.type).AsQueryable();
            #region Filtering
            if (!string.IsNullOrEmpty(search))
            {
                allProducts = allProducts.Where(p => p.NameProduct.Contains(search));
            }
            if (from.HasValue)
            {
                allProducts = allProducts.Where(p => p.Price >= from);
            }
            if (to.HasValue)
            {
                allProducts = allProducts.Where(p => p.Price <= to);
            }
            #endregion
            #region sorting
            //default sort by name
            allProducts = allProducts.OrderBy(p => p.NameProduct);
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc":
                        allProducts = allProducts.OrderByDescending(p => p.NameProduct);
                        break;
                    case "price_asc":
                        allProducts = allProducts.OrderBy(p => p.Price);
                        break;
                    case "price_desc":
                        allProducts = allProducts.OrderByDescending(p => p.Price);
                        break;
                }
            }

            #endregion

            var result = PaginatedList<MyWebApiApp.Data.Product>.create(allProducts, page, PAGE_SIZE);
            return result.Select(p => new ProductModel
            {
                Id = p.ProductId,
                Name = p.NameProduct,
                Price = p.Price,
                NameType = p.type?.NameType
            }).ToList();
        }
    }
}
