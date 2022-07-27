using ApiMedicalClinicEx.Server.Model;
using ApiMedicalClinicEx.Server.Context.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ApiMedicalClinicEx.Server.Services;

public interface IAuthService
{
    AuthModel GetToken(User user, IEnumerable<Claim> Claims);

}   

internal class AuthService : IAuthService
{
    public IConfiguration _Configuration { get; }

    public AuthService(IConfiguration configuration)
    {
        _Configuration = configuration;
    }

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
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.IdDoctor),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
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