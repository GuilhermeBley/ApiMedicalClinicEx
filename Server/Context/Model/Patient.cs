using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [StringLength(3)]
    [ForeignKey(nameof(BloodTypeFk))]
    public string? BloodType { get; set; }

    public BloodType? BloodTypeFk { get; set; }
}