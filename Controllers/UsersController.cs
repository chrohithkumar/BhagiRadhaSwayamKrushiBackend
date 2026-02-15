using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BhagiRadhaSwayamKrushi.Data;
using BhagiRadhaSwayamKrushi.Models;

namespace BhagiRadhaSwayamKrushi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        // Used to load the list in your User Management page
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .OrderBy(u => u.Name)
                .ToListAsync();
            return Ok(users);
        }

        [HttpPut("update-active-status/{mobile}")]
        public async Task<IActionResult> UpdateUserStatus(string mobile, [FromBody] string status)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Mobile == mobile);

            if (user == null)
                return NotFound(new { message = "User not found" });

            // Admin command is applied here: "Active" or "Blocked"
            user.ActiveStatus = status;

            await _context.SaveChangesAsync();

            return Ok(new { message = $"User {user.Name} is now {status}" });
        }

       

    }
}