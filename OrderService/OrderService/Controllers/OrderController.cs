using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

namespace OrderService.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // GET api/orders/1
        [HttpGet("{id}")]
        public ActionResult<OrderViewModel> Get(int id)
        {
            return DummyData.GetOrders(id);
        }
    }
}