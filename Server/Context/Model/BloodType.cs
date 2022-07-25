using System.ComponentModel.DataAnnotations;

namespace ApiMedicalClinicEx.Server.Context.Model;

public class BloodType
{
    [Key]
    [StringLength(3)]
    public string? Desc { get; set; }
}