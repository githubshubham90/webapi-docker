using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    public class DummyData
    {
        public static OrderViewModel GetOrders(int userId)
        {
            var orders = GetDummyData();
            var result = new OrderViewModel
            {
                Orders = orders.Where(a => a.UserId == userId).ToList()
            };
            return result;
        }

        private static List<Order> GetDummyData()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    OrderId=1,
                    UserId=1,
                    OrderAmount=250,
                    OrderDate="14-Apr-2020"
                },
                new Order
                {
                    OrderId=2,
                    UserId=1,
                    OrderAmount=450,
                    OrderDate="15-Apr-2020"
                }
            };

            return orders;
        }
    }
}
