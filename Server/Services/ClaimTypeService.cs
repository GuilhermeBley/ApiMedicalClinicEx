using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiMedicalClinicEx.Server.Services;

/// <summary>
/// Manage claims
/// </summary>
public interface IClaimTypeService
{
    /// <summary>
    /// Get all claims
    /// </summary>
    /// <returns></returns>
    IEnumerable<string?> GetClaims();
}

/// <summary>
/// Stores a claims type in publics constants
/// </summary>
public class ClaimTypeService : IClaimTypeService
{
    /// <inheritdoc cref="JwtRegisteredClaimNames.Name" path="*"/>
    public const string Name = JwtRegisteredClaimNames.Name;

    /// <inheritdoc cref="JwtRegisteredClaimNames.UniqueName" path="*"/>
    public const string UniqueName = JwtRegisteredClaimNames.UniqueName;

    /// <inheritdoc cref="JwtRegisteredClaimNames.Email" path="*"/>
    public const string Email = JwtRegisteredClaimNames.Email;

    /// <inheritdoc cref="JwtRegisteredClaimNames.NameId" path="*"/>
    public const string NameId = JwtRegisteredClaimNames.NameId;

    /// <inheritdoc cref="ClaimTypes.MobilePhone" path="*"/>
    public const string MobilePhone = "mobilephone";

    /// <inheritdoc cref="ClaimTypes.Role" path="*"/>
    public const string Role = ClaimTypes.Role;

    public const string Acess = "acess";

    public IEnumerable<string?> GetClaims()
    {
        return this.GetType().GetFields().Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.IsPublic).Select(fi => fi.Name);
    }

    /// <summary>
    /// Project claims
    /// </summary>
    public static class Claim
    {
        public const string Admin = "admin";
        public const string Commom = "commom";
    }

    /// <summary>
    /// Project Policy
    /// </summary>
    public static class Policy
    {
        public const string OnlyAdm = "onlyadm";
    }
}