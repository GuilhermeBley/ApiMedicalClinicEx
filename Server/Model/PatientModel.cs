using System.ComponentModel.DataAnnotations;

namespace ApiMedicalClinicEx.Server.Model;

public class PatientModel
{
    [Required]
    [RegularExpression(@"([0-9]{11})", ErrorMessage = "Only CPF numbers. Ex '00011122233'.")]
    public string? Cpf { 
        get { 
            return $"{(Cpf is null ? "" : Cpf.SkipLast(4))}"; 
        } 
        set { Cpf = value; } 
    }

    [Required]
    public string? Name { get; set; }

    public DateTime DateCreate { get; set; } = DateTime.Now;

    [Required]
    public string? BloodType { get; set; }
}