namespace ApiMedicalClinicEx.Server.Identity;

internal static class DefaultPolicies
{
    public const string 
        PolicyAdm = nameof(PolicyAdm),
        RoleAdm = "admin",
        RoleCommom = "commom",
        ClaimViewUser = "roles";
}