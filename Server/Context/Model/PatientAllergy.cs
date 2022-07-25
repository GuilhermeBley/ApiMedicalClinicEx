using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMedicalClinicEx.Server.Context.Model;

public class PatientAllergy
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(11)]
    [ForeignKey(nameof(Patient))]
    public string? Cpf { get; set; }

    [Required]
    [StringLength(250)]
    public string? Desc { get; set; }

    public Patient? Patient { get; set; }
}