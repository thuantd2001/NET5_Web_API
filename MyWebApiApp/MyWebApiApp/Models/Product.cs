using System;

namespace MyWebApiApp.Models
{
    public class ProductVM
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }
    public class Product : ProductVM
    {
        public Guid Id { get; set; }
    }
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string NameType { get; set; }
    }
}
