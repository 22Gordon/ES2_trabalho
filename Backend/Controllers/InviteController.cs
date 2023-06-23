using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InviteController : ControllerBase
    {
        private readonly TasksDbContext _context;

        public InviteController(TasksDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendInvite(Guid projectId, string userName)
        {
            try
            {
                // Verificar se o projeto existe
                var project = await _context.Projects.FindAsync(projectId);

                if (project == null)
                {
                    return NotFound("Projeto não encontrado.");
                }

                // Verificar se o utilizador existe como freelancer
                var freelancer = await _context.Freelancers
                    .Include(f => f.User)
                    .FirstOrDefaultAsync(f => f.User.Username == userName);

                if (freelancer != null)
                {
                    // Verificar se o convite já existe para o projeto e freelancer
                    var existingInvite = await _context.Invites
                        .FirstOrDefaultAsync(invite => invite.Projectid == projectId && invite.Freelancerid == freelancer.Userid);

                    if (existingInvite != null)
                    {
                        return Conflict("O convite já foi enviado para o freelancer.");
                    }

                    // Criar um novo convite
                    var invite = new Invite
                    {
                        Projectid = projectId,
                        Freelancerid = freelancer.Userid
                    };

                    _context.Invites.Add(invite);
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                
                return NotFound("Freelancer não encontrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }
        
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetInvitesByUser(Guid userId)
        {
            try
            {
                var invites = await _context.Invites
                    .Include(invite => invite.Project)
                    .Include(invite => invite.Freelancer)
                    .ThenInclude(freelancer => freelancer.User)
                    .Where(invite => invite.Freelancer.Userid == userId)
                    .ToListAsync();

                var inviteViewModels = invites.Select(invite => new InviteViewModel
                {
                    ProjectName = invite.Project.Name,
                    IsAccepted = invite.Isaccepted
                }).ToList();

                return Ok(inviteViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("project/{pid}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetInvitesByProject(Guid pid)
        {
            return await _context.Invites
                .Where(i => i.Projectid == pid)
                .ToListAsync();

        }
        
        [HttpPost("accept")]
        public async Task<IActionResult> AcceptInvite(AcceptInviteModel model)
        {
            try
            {
                var invite =  _context.Invites
                    .Include(i => i.Project)
                    .FirstOrDefault(i => i.Project.Name == model.ProjectTitle && i.Freelancerid == model.UserId);

                if (invite != null && !invite.Isaccepted)
                {
                    invite.Isaccepted = true;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid invitation or already accepted.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpDelete("{pid}/{fid}")]
        public IActionResult RemoveInvite(Guid pid, Guid fid)
        {
            var inv = _context.Invites.FirstOrDefault(i => i.Projectid == pid && i.Freelancerid == fid);
            if (inv == null)
            {
                return NotFound();
            }

            _context.Invites.Remove(inv);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
