using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ApiMedicalClinicEx.Server.Attributes;
using ApiMedicalClinicEx.Server.Identity;

namespace ApiMedicalClinicEx.Server.Controllers;

[ApiController, Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UserController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    #region Users Emails

    [HttpGet, ClaimsAuthorize(DefaultClaimTypes.Access,"viewUsers")]
    public async Task<ActionResult<IEnumerable<string>>> GetUsersEmail()
    {
        return Ok(
            (await _userManager.Users.ToListAsync()).Select(s => s.Email)
        );
    }

    #endregion

    #region User Roles

    [HttpPost("Role")]
    public async Task<ActionResult> CreateUserRole([FromBody] UserRoleCreateModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId.ToString());

        var result = await _userManager.AddToRoleAsync(user, model.RoleName);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok();
    }

    [HttpDelete("Role")]
    public async Task<ActionResult> DeleteUserRole([FromQuery] int userId, [FromQuery] string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return NotFound("Usuário não encontrado.");

        var claim = (await _userManager.GetRolesAsync(user)).FirstOrDefault(f => f == roleName);

        if (string.IsNullOrEmpty(claim))
            return BadRequest("Claim não encontrada.");

        var result = await _userManager.RemoveFromRoleAsync(user, claim);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok();
    }

    [HttpGet("Role")]
    public async Task<ActionResult<IEnumerable<string>>> GetUserRoles([FromQuery] int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return NotFound("Usuário não encontrado.");

        return (await _userManager.GetRolesAsync(user)).ToList();
    }

    #endregion

    #region User Claims

    [HttpPost("Claim")]
    public async Task<ActionResult> CreateUserClaim([FromBody] UserClaimCreateModel userClaimCreateModel)
    {
        var user = await _userManager.FindByIdAsync(userClaimCreateModel.UserId.ToString());

        if (user is null)
        {
            return NotFound("Usuário não encontrado.");
        }

        var result = await _userManager.AddClaimAsync(
            user,
            new System.Security.Claims.Claim(userClaimCreateModel.ClaimType, userClaimCreateModel.ClaimValue)
        );

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok();
    }

    [HttpDelete("Claim")]
    public async Task<ActionResult> DeleteUserClaim([FromQuery] int userId, [FromQuery] string claimType, [FromQuery] string claimValue)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return NotFound("Usuário não encontrado.");

        var claim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(f => f.Type == claimType && f.Value == claimValue);

        if (claim is null)
            return NotFound("Claim não encontrada.");

        var result = await _userManager.RemoveClaimAsync(user, claim);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok();
    }
    #endregion
}