using System;

namespace MyWebApiApp.Data
{
    public class OrderDetail
    {
        public Guid OrderID { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public byte Discount { get; set; }
        //relationship 
        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}
