using System;
using System.Collections.Generic;

namespace MyWebApiApp.Data
{
    public enum StatusOrder
    {
        New = 0, Payment = 1, Complete = 2, Cancell = -1
    }
    public class Order
    {
        public Guid OrderID { get; set; }
        public DateTime DateOrder { get; set; }
        public DateTime? ShipDate { get; set; }
        public StatusOrder StatusOrder { get; set; }
        public string NameGuess { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

    }
}
