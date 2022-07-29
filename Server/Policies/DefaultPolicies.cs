namespace ApiMedicalClinicEx.Server.Policies;

internal static class DefaultPolicies
{
    public const string 
        PolicyAdm = nameof(PolicyAdm),
        RoleAdm = "admin",
        RoleCommom = "commom";
    
    public static (string Type,string Value) ClaimRolesViewUsers { get; }= ("roles", "viewUsers");
}