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
    public class ClientsController : ControllerBase
    {
        private readonly TasksDbContext _context;

        public ClientsController(TasksDbContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetClients()
        {
            return await _context.Clients.Select(c => new
            {
                c.User.Userid,
                c.User.Displayname,
                c.User.Username,
                c.User.Password
            }).ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetClient(Guid id)
        {
            var client = await _context.Clients
                .Include(c=>c.User)
                .FirstOrDefaultAsync(c=> c.Userid==id);

            if (client == null)
            {
                return NotFound();
            }

            var c = new
            {
                client.User?.Userid,
                client.User?.Displayname,
                client.User?.Username,
                client.User?.Password
            };

            return c;
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(Guid id, Client client)
        {
            if (id != client.Userid)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Userid }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(Guid id)
        {
            return _context.Clients.Any(e => e.Userid == id);
        }
        
        // POST: api/Clients/Register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterClient([FromBody] User user)
        {
            // Crie um novo objeto User
            var newUser = new User
            {
                Userid = user.Userid,
                Displayname = user.Displayname,
                Username = user.Username,
                Password = user.Password
            };

            // Crie um novo objeto Client associado ao utilizadr
            var client = new Client
            {
                User = newUser,
                Userid = newUser.Userid
            };

            // Adicione o cliente e o utilizador ao contexto da BD
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Userid }, client);
        }
    }
}
