using BusinessLogic.Context;
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

        [HttpGet("/{projectId}/{taskId}")]
        public IActionResult AddTaskToProject(Guid projectId, Guid taskId)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Projectid == projectId);
            var task = _context.UserTasks.FirstOrDefault(t => t.Taskid == taskId);

            // Check if both project and task exist
            if (project != null && task != null)
            {
                // Add the task to the project's list of tasks
                project.Tasks.Add(task);
                // Save changes to your data store
                _context.SaveChanges();
                return Ok("Task added to project successfully");
            }
            // If either project or task is not found, return an error response
            return NotFound();
        }
    }
}