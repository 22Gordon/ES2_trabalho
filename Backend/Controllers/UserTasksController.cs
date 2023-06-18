using BusinessLogic.Context;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTasksController : ControllerBase
    {
        private readonly TasksDbContext _context;

        public UserTasksController(TasksDbContext context)
        {
            _context = context;
        }
        
        // GET: api/usertasks
        // List all user tasks
        [HttpGet]
        public IActionResult GetAllUserTasks()
        {
            var tasks = _context.UserTasks.ToList();
            return Ok(tasks);
        }
        
        // GET api/usertasks/user/{userId}
        // List user tasks based on userId
        [HttpGet("user/{userId}")]
        public IActionResult GetUserTasks(Guid userId)
        {
            var userTasks = _context.UserTasks.Where(task => task.Clientid == userId || task.Freelancerid == userId).ToList();
            return Ok(userTasks);
        }
        
        // GET api/usertasks/{id}
        // List specific user tasks, based on id
        [HttpGet("{id}")]
        public IActionResult GetUserTaskById(Guid id)
        {
            var task = _context.UserTasks.FirstOrDefault(t => t.Taskid == id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        // PUT api/usertasks/{id}
        // Update db specific user task, based on id
        [HttpPut("{id}")]
        public IActionResult UpdateUserTask(Guid id, UserTask updatedTask)
        {
            var task = _context.UserTasks.FirstOrDefault(t => t.Taskid == id);
            if (task == null)
            {
                return NotFound();
            }

            // Update task properties
            task.Clientid = updatedTask.Clientid;
            task.Freelancerid = updatedTask.Freelancerid;
            task.Startdate = updatedTask.Startdate;
            task.Pricehour = updatedTask.Pricehour;
            task.Enddate = updatedTask.Enddate?.ToUniversalTime();

            // Calculate duration if both Startdate and Enddate are specified
            if (task.Startdate.HasValue && task.Enddate.HasValue)
            {
                TimeSpan duration = task.Enddate.Value - task.Startdate.Value;
                task.Duration = duration;
            }

            _context.SaveChanges();
            return NoContent();
        }
        
        // POST api/projects
        // Creates a db entity of a project
        [HttpPost]
        public IActionResult CreateTask(UserTask task)
        {
            // Converter Startdate para UTC, se não estiver especificado como UTC
            if (task.Startdate.HasValue && task.Startdate.Value.Kind != DateTimeKind.Utc)
            {
                task.Startdate = task.Startdate.Value.ToUniversalTime();
            }

            _context.UserTasks.Add(task);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserTaskById), new { id = task.Taskid }, task);
        }

        
        
        // DELETE api/usertasks/{id}
        // Deletes an entity from the database
        [HttpDelete("{id}")]
        public IActionResult DeleteUserTask(Guid id)
        {
            var task = _context.UserTasks.FirstOrDefault(t => t.Taskid == id);
            if (task == null)
            {
                return NotFound();
            }

            _context.UserTasks.Remove(task);
            _context.SaveChanges();
            return NoContent();
        }
        
    }
}
