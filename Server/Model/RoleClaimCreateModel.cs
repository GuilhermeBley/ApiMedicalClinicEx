using System.ComponentModel.DataAnnotations;

namespace ApiMedicalClinicEx.Server.Model;

public class RoleClaimCreateModel
{
    public int RoleId { get; set; }

    [Required, StringLength(50, MinimumLength = 5, ErrorMessage = "Tamanho deve ser entre 5 - 50")]
    public string ClaimType { get; set; } = null!;

    [Required, StringLength(50, MinimumLength = 5, ErrorMessage = "Tamanho deve ser entre 5 - 50")]
    public string ClaimValue { get; set; } = null!;
}