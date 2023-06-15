using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public async Task<ActionResult<IEnumerable<dynamic>>> getProjects()
        {
            return await _context.Projects.Select(p => new
            {
                p.Projectid,
                p.Name,
                p.Pricehour,
                Client = new
                {
                    p.Client.Userid,
                    p.Client.User.Displayname
                },
                Projectleader = new
                {
                    p.Projectleader.Userid,
                    p.Projectleader.User.Displayname,
                    p.Projectleader.Dailyavghours   
                },
                Freelancers = p.Freelancers.Select(f => new
                {
                    f.Userid,
                    f.User.Displayname,
                    f.Dailyavghours
                }),
                UserTasks = p.UserTasks.Select(t => new
                {
                    t.Taskid,
                    t.Freelancer.User.Displayname,
                    t.Startdate,
                    t.Enddate,
                    t.Pricehour
                })
            }).ToListAsync();
        }
        
        // GET api/projects/{id}
        // List specific project, based on id
        [HttpGet("{id}")]
        public IActionResult GetProjectById(Guid id)
        {
            var p = _context.Projects
                .Include(p => p.Freelancers)
                .ThenInclude(f => f.User)
                .Include(p => p.UserTasks)
                .ThenInclude(t => t.Freelancer)
                .ThenInclude(f => f.User)
                .Include(p => p.Client)
                .ThenInclude(c => c.User)
                .FirstOrDefault(p => p.Projectid == id);
    
            if (p == null)
            {
                return NotFound();
            }

            var project = new
            {
                Projectid = p?.Projectid,
                Name = p?.Name,
                Pricehour = p?.Pricehour,
                Client = new
                {
                    Userid = p?.Client?.Userid,
                    Displayname = p?.Client?.User?.Displayname
                },
                Projectleader = new
                {
                    Userid = p?.Projectleader?.Userid,
                    Displayname = p?.Projectleader?.User?.Displayname,
                    Dailyavghours = p?.Projectleader?.Dailyavghours
                },
                Freelancers = p.Freelancers.Select(f => new
                {
                    Userid = f?.Userid,
                    Displayname = f?.User?.Displayname,
                    Dailyavghours = f?.Dailyavghours
                }),
                UserTasks = p.UserTasks.Select(t => new
                {
                    Taskid = t?.Taskid,
                    Displayname = t?.Freelancer?.User?.Displayname,
                    t?.Startdate,
                    t?.Enddate,
                    t?.Pricehour
                })
            };
    
            return Ok(project);
        }
        
        
        // GET api/projects/{client}
        // List specific project, based on client
        [HttpGet("client/{client}")]
        public IActionResult GetProjectByCli(Guid client)
        {
            var p = _context.Projects
                .Include(p => p.Freelancers)
                .ThenInclude(f => f.User)
                .Include(p => p.UserTasks)
                .ThenInclude(t => t.Freelancer)
                .ThenInclude(f => f.User)
                .Include(p => p.Client)
                .ThenInclude(c => c.User)
                .FirstOrDefault(p => p.Client.Userid == client);
    
            if (p == null)
            {
                return NotFound();
            }

            var project = new
            {
                Projectid = p?.Projectid,
                Name = p?.Name,
                Pricehour = p?.Pricehour,
                Client = new
                {
                    Userid = p?.Client?.Userid,
                    Displayname = p?.Client?.User?.Displayname
                },
                Projectleader = new
                {
                    Userid = p?.Projectleader?.Userid,
                    Displayname = p?.Projectleader?.User?.Displayname,
                    Dailyavghours = p?.Projectleader?.Dailyavghours
                },
                Freelancers = p.Freelancers.Select(f => new
                {
                    Userid = f?.Userid,
                    Displayname = f?.User?.Displayname,
                    Dailyavghours = f?.Dailyavghours
                }),
                UserTasks = p.UserTasks.Select(t => new
                {
                    Taskid = t?.Taskid,
                    Displayname = t?.Freelancer?.User?.Displayname,
                    t?.Startdate,
                    t?.Enddate,
                    t?.Pricehour
                })
            };
    
            return Ok(project);
        }
        
        
        // GET api/projects/{freelancer}
        // List specific project, based on freelancer
        [HttpGet("freelancer/{freelancer}")]
        public IActionResult GetProjectByFree(Guid freelancer)
        {
            var p = _context.Projects
                .Include(p => p.Freelancers)
                .ThenInclude(f => f.User)
                .Include(p => p.UserTasks)
                .ThenInclude(t => t.Freelancer)
                .ThenInclude(f => f.User)
                .Include(p => p.Client)
                .ThenInclude(c => c.User)
                .FirstOrDefault(p => p.Projectleaderid == freelancer || p.Freelancers.Any(f => f.Userid == freelancer));
    
            if (p == null)
            {
                return NotFound();
            }

            var project = new
            {
                Projectid = p?.Projectid,
                Name = p?.Name,
                Pricehour = p?.Pricehour,
                Client = new
                {
                    Userid = p?.Client?.Userid,
                    Displayname = p?.Client?.User?.Displayname
                },
                Projectleader = new
                {
                    Userid = p?.Projectleader?.Userid,
                    Displayname = p?.Projectleader?.User?.Displayname,
                    Dailyavghours = p?.Projectleader?.Dailyavghours
                },
                Freelancers = p.Freelancers.Select(f => new
                {
                    Userid = f?.Userid,
                    Displayname = f?.User?.Displayname,
                    Dailyavghours = f?.Dailyavghours
                }),
                UserTasks = p.UserTasks.Select(t => new
                {
                    Taskid = t?.Taskid,
                    Displayname = t?.Freelancer?.User?.Displayname,
                    t?.Startdate,
                    t?.Enddate,
                    t?.Pricehour
                })
            };
    
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
            project.Clientid = updatedProject.Clientid;
            project.Projectleaderid = updatedProject.Projectleaderid;
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
