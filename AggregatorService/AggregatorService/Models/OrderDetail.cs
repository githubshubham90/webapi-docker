using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AggregatorService.Models
{
    public class OrderDetail : OrderViewModel
    {
        public User UserDetails { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public long OrderAmount { get; set; }
        public string OrderDate { get; set; }
    }

    public class OrderViewModel
    {
        public List<Order> Orders { get; set; }
    }
}
