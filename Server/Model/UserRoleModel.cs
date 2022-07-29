using System.ComponentModel.DataAnnotations;

namespace ApiMedicalClinicEx.Server.Model;

public class UserRoleCreateModel
{
    [Required]
    public int? UserId { get; set; }

    [Required]
    public string RoleName { get; set; } = null!;
}