using AIInterviewCoach.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AIInterviewCoach.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet("pending-mentors")]
    public async Task<IActionResult> GetPendingMentors()
    {
        var pendingMentors = await _userManager.GetUsersInRoleAsync("PendingMentor");
        return Ok(pendingMentors.Select(u => new { u.Id, u.Email }));
    }

    [HttpPost("approve-mentor/{userId}")]
    public async Task<IActionResult> ApproveMentor(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("User not found.");

        await _userManager.RemoveFromRoleAsync(user, "PendingMentor");
        
        if (!await _roleManager.RoleExistsAsync("Mentor"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Mentor"));
        }

        if (!await _userManager.IsInRoleAsync(user, "Mentor"))
        {
            await _userManager.AddToRoleAsync(user, "Mentor");
        }

        return Ok();
    }
}
