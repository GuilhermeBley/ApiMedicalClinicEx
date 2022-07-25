using System.ComponentModel.DataAnnotations;

namespace ApiMedicalClinicEx.Server.Context.Model;

public class Patient
{
    [Key]
    [StringLength(11)]
    public string? Cpf { get; set; }

    [Required]
    [StringLength(250)]
    public string? Name { get; set; }

    [Required]
    public DateTime? DateCreate { get; set; } = DateTime.Now;

    // nullable
    [StringLength(3)]
    public string? BloodType { get; set; }
}