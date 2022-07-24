using Microsoft.AspNetCore.Identity;

namespace ApiMedicalClinicEx.Server.Context.Model;

/// <summary>
/// Roles with user
/// </summary>
public class UserToken : IdentityUserToken<int>
{
}