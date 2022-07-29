using System.ComponentModel.DataAnnotations;
using ApiMedicalClinicEx.Server.Context;
using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Filter;
using ApiMedicalClinicEx.Server.Model;
using ApiMedicalClinicEx.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMedicalClinicEx.Server.Controllers;

[ApiController, Route("api/[controller]")]
[TypeFilter(typeof(DevOnlyActionFilter))]
public class PolicyController : ControllerBase
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public PolicyController(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    #region Role

    [HttpPost("Role")]
    public async Task<IActionResult> CreateRole([FromBody] RoleCreateModel roleModel)
    {
        IdentityResult result = await _roleManager.CreateAsync(new Role { Name = roleModel.RoleName });

        if (result.Succeeded)
        {
            return Ok();
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    [HttpGet("Role")]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        return await _roleManager.Roles.ToListAsync();
    }

    [HttpDelete("Role")]
    public async Task<IActionResult> DeleteRole([FromQuery] string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);

        if (role is null)
            return NotFound("Role não encontrada.");

        await _roleManager.DeleteAsync(role);

        return Ok();
    }

    [HttpPost("Role/Claim")]
    public async Task<ActionResult> CreateRoleClaim([FromBody] RoleClaimCreateModel claimModel)
    {
        var role = await _roleManager.FindByIdAsync(claimModel.RoleId.ToString());

        if (role is null)
            return NotFound("Role não encontrada");

        var result = await _roleManager.AddClaimAsync(
            role,
            new System.Security.Claims.Claim(claimModel.ClaimType, claimModel.ClaimValue)
        );

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok();
    }

    [HttpDelete("Role/Claim")]
    public async Task<ActionResult> DeleteClaim([FromQuery] int roleId, [FromQuery] string claimType, [FromQuery] string claimValue)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());

        if (role is null)
            return NotFound("Role não encontrada");

        var claim = (await _roleManager.GetClaimsAsync(role)).FirstOrDefault(f => f.Type == claimType && f.Value == claimValue);

        if (claim is null)
            return NotFound("Claim não encontrada");

        var result = await _roleManager.RemoveClaimAsync(role, claim);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    #endregion
}