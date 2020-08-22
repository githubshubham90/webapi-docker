using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AggregatorService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AggregatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        // GET api/orderdetails/1
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> Get(int id)
        {
            var orderDetails = new OrderDetail
            {
                UserDetails = await GetUserDetails(id),
                Orders = await GetOrders(id)
            };
            return orderDetails;
        }

        private async Task<User> GetUserDetails(int id)
        {
            string Baseurl = Environment.GetEnvironmentVariable("USERAPI_URL") ?? "http://localhost:8090/";
            Console.WriteLine("User Service Base Url: {0}", Baseurl);
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var result = await client.GetStringAsync("api/user/" + id.ToString());
                    return JsonConvert.DeserializeObject<User>(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new User();
            }            
        }

        private async Task<List<Order>> GetOrders(int id)
        {
            string Baseurl = Environment.GetEnvironmentVariable("ORDERAPI_URL") ?? "http://localhost:8091/";
            Console.WriteLine("Order Service Base Url: {0}", Baseurl);
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var result = await client.GetStringAsync("api/orders/" + id.ToString());
                    return JsonConvert.DeserializeObject<OrderViewModel>(result).Orders;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Order>();
            }            
        }
    }
}