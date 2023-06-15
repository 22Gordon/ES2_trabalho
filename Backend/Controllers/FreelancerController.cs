using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Context;
using BusinessLogic.Entities;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancersController : ControllerBase
    {
        private readonly TasksDbContext _context;

        public FreelancersController(TasksDbContext context)
        {
            _context = context;
        }

        // GET: api/Freelancers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetFreelancers()
        {
            return await _context.Freelancers.Select(f => new
            {
                f.User.Userid,
                f.User.Displayname,
                f.User.Username,
                f.User.Password
            }).ToListAsync();
        }

        // GET: api/Freelancers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetFreelancer(Guid id)
        {
            var freelancer = await _context.Freelancers
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Userid==id);

            if (freelancer == null)
            {
                return NotFound();
            }
            
            var f = new
            {
                freelancer.User?.Userid,
                freelancer.User?.Displayname,
                freelancer.User?.Username,
                freelancer.User?.Password
            };

            return f;
        }

        // PUT: api/Freelancers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFreelancer(Guid id, Freelancer freelancer)
        {
            if (id != freelancer.Userid)
            {
                return BadRequest();
            }

            _context.Entry(freelancer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FreelancerExists(id))
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

        // POST: api/Freelancers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Freelancer>> PostFreelancer(Freelancer freelancer)
        {
            _context.Freelancers.Add(freelancer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFreelancer", new { id = freelancer.Userid }, freelancer);
        }

        // DELETE: api/Freelancers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFreelancer(Guid id)
        {
            var freelancer = await _context.Freelancers.FindAsync(id);
            if (freelancer == null)
            {
                return NotFound();
            }

            _context.Freelancers.Remove(freelancer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FreelancerExists(Guid id)
        {
            return _context.Freelancers.Any(e => e.Userid == id);
        }
        
        // POST: api/Freelancers/Register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterFreelancer(User user)
        {
            // Crie um novo objeto User
            var newUser = new User
            {
                Userid = user.Userid,
                Displayname = user.Displayname,
                Username = user.Username,
                Password = user.Password
            };

            // Crie um novo objeto Freelancer associado ao utilizadr
            var freelancer = new Freelancer
            {
                User = newUser,
                Userid = newUser.Userid,
                Dailyavghours = 0
            };

            // Adicione o freelancer e o utilizador ao contexto da BD
            _context.Freelancers.Add(freelancer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFreelancer", new { id = freelancer.Userid }, freelancer);
        }
    } 
}
