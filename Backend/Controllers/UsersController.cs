using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TasksDbContext _context;

        public UsersController(TasksDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        
        // GET: api/Users/project/5
        [HttpGet("project/{pid}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetUsersFromProject(Guid pid)
        {
            var list = _context.Invites
                .Where(i => i.Projectid == pid && i.Isaccepted)
                .Select(i => i.Freelancerid)
                .ToList();

            var project = _context.Projects.Where(p => p.Projectid == pid).Select(p => p).ToList();
            var p = project[0];
            if (p != null)
            {
                if (p.Clientid != null)
                {
                    list.Insert(0, (Guid)p.Clientid);
                }
                if (p.Projectleaderid != null)
                {
                    list.Insert(0, (Guid)p.Projectleaderid);
                }
            }

            return await _context.Users
                .Where(u => list.Contains(u.Userid))
                .ToListAsync();
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Userid)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        // PUT: api/Users/edit/{username}
        [HttpPut("edit/{username}")]
        public async Task<IActionResult> PutUserProfile(string username, UserEdit userEdit)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Displayname = userEdit.Displayname;
            existingUser.Username = userEdit.Username;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(existingUser.Userid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Userid }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Userid == id);
        }
        
        [HttpPut("changepassword/{username}")]
        public async Task<IActionResult> ChangePassword(string username, [FromBody] PasswordUpdateModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            // Verificar se a senha atual fornecida está correta
            if (user.Password != model.CurrentPassword)
            {
                return BadRequest("Invalid current password");
            }

            // Atualizar a senha do utilizador
            user.Password = model.NewPassword;
            await _context.SaveChangesAsync();

            return Ok();
        }
        
        // GET: api/Users/CheckUsername/{username}
        [HttpGet("CheckUsername/{username}")]
        public async Task<ActionResult<bool>> CheckUsername(string username)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return existingUser == null;
        }
    }
}