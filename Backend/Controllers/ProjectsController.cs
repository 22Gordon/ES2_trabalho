using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ProjectsController(MyDbContext context)
        {
            _context = context;
        }
        
        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> getProjects()
        {
            return await _context.Projects.Select(p => new
            {
                p.Projectid,
                p.Name,
                p.Pricehour,
                Client = new
                {
                    p.Client.Userid
                },
                Projectleader = new
                {
                    p.Projectleader.Userid,
                    p.Projectleader.Dailyavghours
                },
                p.Freelancers,
                p.UserTasks
            }).ToListAsync();
        }
    }
}
