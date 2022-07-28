using ApiMedicalClinicEx.Server.Context.Model;
using ApiMedicalClinicEx.Server.Model;
using ApiMedicalClinicEx.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiMedicalClinicEx.Server.Controllers;

[ApiController, Route("api/[controller]")]
public class LoginController : ControllerBase
{
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthService _authService;

        public LoginController(UserManager<User> userManager, SignInManager<User> signInManager, IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }        


    [HttpGet, Authorize]
    public async Task<IActionResult> Get()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("register"), AllowAnonymous]
    public async Task<ActionResult<AuthModel>> Post([FromBody] LoginRegisterModel userLogin)
    {
        var user = await _userManager.FindByEmailAsync(userLogin.Login);

        if (user == null)
        {
            return Unauthorized();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

        if (result.IsNotAllowed)
        {
            return Unauthorized();
        }

        if (result.Succeeded)
        {
            var claims = await _userManager.GetClaimsAsync(user);

            return Ok(_authService.GetToken(user, claims));
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpPost("create"), AllowAnonymous]
    public async Task<ActionResult<AuthModel>> CreateUser([FromBody] LoginCreateModel userCreate)
    {
            if (string.IsNullOrEmpty(userCreate.Email))
                return BadRequest(nameof(userCreate.Email));

            
            if (await _userManager.FindByEmailAsync(userCreate.Email) is not null)
            {
                return BadRequest("Usuário já existente.");
            }

            var user = new User { 
                Email = userCreate.Email,
                UserName = userCreate.Email,
                NormalizedUserName = userCreate.Email,
                IdDoctor = Guid.NewGuid().ToString(),
                PhoneNumber = userCreate.Phone 
            };

            var result = await _userManager.CreateAsync(user, userCreate.Password);

            if (result.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(userCreate.Email);

                if (user is null)
                    return BadRequest("Falha ao criar usuário.");

                var claims = await _userManager.GetClaimsAsync(user);

                return Ok(_authService.GetToken(user, claims));
            }
            else
            {
                return BadRequest("Senha requer letra maiúscula, minúscula, caracteres numéricos e especiais.");
            }
        }
}