using ApiMedicalClinicEx.Server.Model;
using ApiMedicalClinicEx.Server.Context.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ApiMedicalClinicEx.Server.Services;

/// <summary>
/// Authentication service, provides token
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Gets Token login
    /// </summary>
    /// <param name="user">user</param>
    /// <param name="Claims">user claims to add in token</param>
    /// <returns>AuthModel</returns>
    AuthModel GetToken(User user, IEnumerable<Claim> Claims = null!);

}   

internal class AuthService : IAuthService
{
    public IConfiguration _Configuration { get; }

    public AuthService(IConfiguration configuration)
    {
        _Configuration = configuration;
    }

    /// <inheritdoc/>
    public AuthModel GetToken(User user, IEnumerable<Claim> Claims = null!)
    {
        if (Claims is null)
            Claims = new List<Claim>();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_Configuration["Jwt:Key"]);

        // Gerando token
        var expires = DateTime.UtcNow.AddHours(8);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            // Claims
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypeService.Name, user.UserName),
                new Claim(ClaimTypeService.NameId, user.Id.ToString()),
                new Claim(ClaimTypeService.UniqueName, user.IdDoctor),
                new Claim(ClaimTypeService.Email, user.Email),
                new Claim(ClaimTypeService.MobilePhone, user.PhoneNumber)
            })
        };

        foreach (var claim in Claims)
            tokenDescriptor.Subject.AddClaim(claim);

        var token = tokenHandler.CreateToken(tokenDescriptor);
            
        return new AuthModel()
        {
            Token = tokenHandler.WriteToken(token),
            Exp = expires,
            User = new UserModel{
                Email = user.Email,
                Id = user.Id,
                IdDoctor = user.IdDoctor,
                Name = user.UserName,
                PhoneNumber = user.PhoneNumber
            }
        };
    }
}