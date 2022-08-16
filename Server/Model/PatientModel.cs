using System.ComponentModel.DataAnnotations;

namespace ApiMedicalClinicEx.Server.Model;

public class PatientModel
{
    [Required]
    [RegularExpression(@"([0-9]{11})", ErrorMessage = "Insira apenas números ao CPF. Ex '00011122233'.")]
    public string? Cpf { get; set; }

    [Required]
    public string? Name { get; set; }

    public DateTime DateCreate { get; set; } = DateTime.Now;

    [Required]
    public string? BloodType { get; set; }
}