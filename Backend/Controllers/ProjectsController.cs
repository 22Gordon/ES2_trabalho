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
        private readonly TasksDbContext _context;

        public ProjectsController(TasksDbContext context)
        {
            _context = context;
        }
        
        // GET: api/Projects
        // List all projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
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
        
        // GET api/projects/user/{id}
        // List all projects, based on user id
        [HttpGet("user/{id:guid}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProjectByUser(Guid id)
        {
            List<Guid> list = _context.Invites
                .Where(i => i.Freelancerid == id && i.Isaccepted)
                .Select(i => i.Projectid).ToList();
            
            return await _context.Projects
                .Where(p => p.Projectleaderid == id || p.Clientid == id || list.Contains(p.Projectid))
                .ToListAsync();
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
        [HttpDelete("{pid}/{b}")]
        public IActionResult DeleteProject(Guid pid, Boolean b)
        {
            if (b)
            {
                var project = _context.Projects.FirstOrDefault(p => p.Projectid == pid);
                if (project == null)
                {
                    return NotFound();
                }
                
                List<Guid> list = _context.Taskprojects
                    .Where(tp => tp.Projectid == pid)
                    .Select(tp => tp.Taskid)
                    .ToList();

                List<Guid> invList = _context.Invites
                    .Where(i => i.Projectid == pid)
                    .Select(i => i.Freelancerid)
                    .ToList();

                var taskProjectController = new TaskProjectController(_context);
                var userTaskController = new UserTasksController(_context);
                var inviteController = new InviteController(_context);
                
                foreach (var tid in list)
                {
                    taskProjectController.RemoveTaskProject(pid, tid);
                    userTaskController.DeleteUserTask(tid);
                }

                foreach (var fid in invList)
                {
                    inviteController.RemoveInvite(pid, fid);
                }

                _context.Projects.Remove(project);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                var project = _context.Projects.FirstOrDefault(p => p.Projectid == pid);
                if (project == null)
                {
                    return NotFound();
                }
                
                List<Guid> list = _context.Taskprojects
                    .Where(tp => tp.Projectid == pid)
                    .Select(tp => tp.Taskid)
                    .ToList();

                List<Guid> invList = _context.Invites
                    .Where(i => i.Projectid == pid)
                    .Select(i => i.Freelancerid)
                    .ToList();

                var taskProjectController = new TaskProjectController(_context);
                var inviteController = new InviteController(_context);
                
                foreach (var tid in list)
                {
                    taskProjectController.RemoveTaskProject(pid, tid);
                }

                foreach (var fid in invList)
                {
                    inviteController.RemoveInvite(pid, fid);
                }
                
                _context.Projects.Remove(project);
                _context.SaveChanges();
                return Ok();
            }
        }
    }
}
