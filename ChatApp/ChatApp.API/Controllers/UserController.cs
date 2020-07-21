using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.API.DataAccess;
using ChatApp.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ChatAppContext _context;
        public UserController(ChatAppContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(User user)
        {
            var u = await _context.User.FirstOrDefaultAsync(x => x.Username == user.Username && x.Password == user.Password);

            if (u != null)
            {
                u.IsSuccess = true;
                return StatusCode(200, u);
            }
            else
            {
                return StatusCode(401);
            }

        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(User user)
        {
            var u = await _context.User.FirstOrDefaultAsync(x => x.Username == user.Username);

            if (u != null)
                return StatusCode(200, "Bu kullanıcı adı alınmış");

            u.IsSuccess = true;

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return StatusCode(200, user);

        }
    }
}
