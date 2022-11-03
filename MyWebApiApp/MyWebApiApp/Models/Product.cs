using System;

namespace MyWebApiApp.Models
{
    public class CategoryProduct
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }
    public class Product : CategoryProduct
    {
        public Guid Id { get; set; }
    }
}
