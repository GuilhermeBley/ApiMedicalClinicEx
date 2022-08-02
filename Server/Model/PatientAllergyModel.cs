using System.ComponentModel.DataAnnotations;

namespace ApiMedicalClinicEx.Server.Model;

public class PatientAllergyModel
{

    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string? Cpf { get; set; }

    [Required]
    [StringLength(250)]
    public string? Desc { get; set; }
}