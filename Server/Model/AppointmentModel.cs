using System.ComponentModel.DataAnnotations;

namespace ApiMedicalClinicEx.Server.Model;

public class AppointmentModel
{
    [Required]
    public int Medic { get; set; }

    [Required]
    public string? Patient { get; set; }

    [Required]
    public DateTime DateAppo { get; set; }

    [Required]
    public string? Desc { get; set; }

    public int? LastAppo { get; set; }
}