using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;

        public UserController(UserContext userContext)
        {
            _context = userContext;
        }

        // GET api/user
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return _context.Users;
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            return _context.Find<User>(id);
        }

        // POST api/user
        [HttpPost]
        public ActionResult<User> Post([FromBody] User model)
        {
            //var user = new User { Name = name, Email = email, Age = age };
            var result = _context.Add(model);
            _context.SaveChanges();
            return result.Entity;
        }



        // DELETE api/user/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var user = _context.Find<User>(id);
            if (user == null)
                return NotFound();
            _context.Remove(user);
            _context.SaveChanges();
            return Ok();
        }
    }
}