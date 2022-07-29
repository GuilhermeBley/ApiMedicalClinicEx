using System.ComponentModel.DataAnnotations;

namespace ApiMedicalClinicEx.Server.Model;

public class RoleCreateModel
{

    [Required, StringLength(50, MinimumLength = 5, ErrorMessage = "Tamanho deve ser entre 5 - 50")]
    public string RoleName { get; set; } = null!;
}