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
        // List all projects
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
        
        // GET api/projects/{id}
        // List specific project, based on id
        [HttpGet("{id}")]
        public IActionResult GetProjectById(Guid id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Projectid == id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
        
        // PUT api/projects/{id}
        // Update db specific project, based on id
        [HttpPut("{id}")]
        public IActionResult UpdateProject(Guid id, Project updatedProject)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Projectid == id);
            if (project == null)
            {
                return NotFound();
            }

            // Update project properties
            project.Name = updatedProject.Name;
            project.Projectleaderid = updatedProject.Projectleaderid;
            project.Clientid = updatedProject.Clientid;
            project.Pricehour = updatedProject.Pricehour;

            _context.SaveChanges();
            return NoContent();
        }
        
        // POST api/projects
        // Creates a db entity of a project
        [HttpPost]
        public IActionResult CreateProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Projectid }, project);
        }
        
        // DELETE api/projects/{id}
        // Deletes an entity from the database
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(Guid id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Projectid == id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
