using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskProjectController : ControllerBase
    {
        private readonly TasksDbContext _context;

        public TaskProjectController(TasksDbContext context)
        {
            _context = context;
        }

        [HttpGet("/{pid}/{tid}")]
        public IActionResult AddTaskToProject(Guid pid, Guid tid)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Projectid == pid);
            var task = _context.UserTasks.FirstOrDefault(t => t.Taskid == tid);

            // Check if both project and task exist
            if (project != null && task != null)
            {
                // Add the task to the project's list of tasks
                var tp = new Taskproject
                {
                    Projectid = pid,
                    Taskid = tid
                };
                // Save changes to your data store
                _context.Taskprojects.Add(tp);
                _context.SaveChanges();
                return Ok("Task added to project successfully");
            }
            // If either project or task is not found, return an error response
            return NotFound();
        }

        [HttpDelete("/{pid}/{tid}")]
        public IActionResult RemoveTaskProject(Guid pid, Guid tid)
        {
            var tp = _context.Taskprojects.FirstOrDefault(tp => tp.Projectid == pid && tp.Taskid == tid);
            if (tp == null)
            {
                return NotFound();
            }

            _context.Taskprojects.Remove(tp);
            _context.SaveChanges();
            return NoContent();
        }
    }
}