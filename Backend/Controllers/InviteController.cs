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
        private readonly TasksDbContext _dbContext;

        public InviteController(TasksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> SendInvite(Guid projectId, string userName)
        {
            try
            {
                // Verificar se o projeto existe
                var project = await _dbContext.Projects.FindAsync(projectId);

                if (project == null)
                {
                    return NotFound("Projeto não encontrado.");
                }

                // Verificar se o utilizador existe como freelancer
                var freelancer = await _dbContext.Freelancers
                    .Include(f => f.User)
                    .FirstOrDefaultAsync(f => f.User.Username == userName);

                if (freelancer != null)
                {
                    // Verificar se o convite já existe para o projeto e freelancer
                    var existingInvite = await _dbContext.Invites
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

                    _dbContext.Invites.Add(invite);
                    await _dbContext.SaveChangesAsync();

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
                var invites = await _dbContext.Invites
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
        
        [HttpPost("accept")]
        public async Task<IActionResult> AcceptInvite(AcceptInviteModel model)
        {
            try
            {
                var invite = await _dbContext.Invites
                    .Include(invite => invite.Project)
                    .SingleOrDefaultAsync(invite => invite.Project.Name == model.ProjectTitle);

                if (invite != null && !invite.Isaccepted)
                {
                    invite.Isaccepted = true;
                    await _dbContext.SaveChangesAsync();
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


    }
}
