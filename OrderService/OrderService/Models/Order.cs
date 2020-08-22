using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public long OrderAmount { get; set; }
        public string OrderDate { get; set; }
    }

    public class OrderViewModel
    {
        public List<Order> Orders { get; set; }
    }
}
