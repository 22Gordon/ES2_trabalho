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
        private readonly MyDbContext _context;

        public UserTasksController(MyDbContext context)
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
            task.Projectid = updatedTask.Projectid;
            task.Freelancerid = updatedTask.Freelancerid;
            task.Startdate = updatedTask.Startdate;
            task.Enddate = updatedTask.Enddate;
            task.Pricehour = updatedTask.Pricehour;

            _context.SaveChanges();
            return NoContent();
        }
        
        // POST api/usertasks
        // Creates a db entity of a UserTask
        [HttpPost]
        public IActionResult CreateUserTask(UserTask task)
        {
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
