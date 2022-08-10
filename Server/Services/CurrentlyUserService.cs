using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiMedicalClinicEx.Server.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiMedicalClinicEx.Server.Services;

/// <summary>
/// currently user service
/// </summary>
public interface ICurrentlyUserService
{
    CurrentlyUserModel GetUser();
}

/// <inheritdoc/>
internal class CurrentlyUserService : ICurrentlyUserService
{
    private CurrentlyUserModel EmptyUser => new();

    private readonly IHttpContextAccessor _contextAcessor;

    public CurrentlyUserService(IHttpContextAccessor contextAcessor)
    {
        _contextAcessor = contextAcessor;
    }

    public CurrentlyUserModel GetUser()
    {
        string token = ((string)_contextAcessor.HttpContext!.Request.Headers.Authorization).Replace(JwtBearerDefaults.AuthenticationScheme+" ","");
            token = ((string)_contextAcessor.HttpContext.Request.Headers.Authorization).Replace(JwtBearerDefaults.AuthenticationScheme.ToLower()+" ","");
            
            if (string.IsNullOrEmpty(token))
                return EmptyUser;

            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token))
            {
                return EmptyUser;
            }

            var claims = tokenHandler.ReadJwtToken(token).Claims;

            var name = claims.FirstOrDefault(predicate => predicate.Type == JwtRegisteredClaimNames.Name);
            var email = claims.FirstOrDefault(predicate => predicate.Type == JwtRegisteredClaimNames.Email);
            var medicId = claims.FirstOrDefault(predicate => predicate.Type == JwtRegisteredClaimNames.UniqueName);
            var identificador = int.TryParse(claims.FirstOrDefault(predicate => predicate.Type == JwtRegisteredClaimNames.NameId)?.Value, out int userId);
            var phoneNumber = claims.FirstOrDefault(predicate => predicate.Type == ClaimTypes.MobilePhone);

            return new CurrentlyUserModel
            {
                MedicalId = medicId is null ? "" : medicId.Value,
                Email = email is null ? "" : email.Value,
                PhoneNumber = phoneNumber is null ? "" : phoneNumber.Value,
                UserId = identificador is false ? -1 : userId
            };
    }
}