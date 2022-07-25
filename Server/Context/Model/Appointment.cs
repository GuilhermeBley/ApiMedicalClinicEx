using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMedicalClinicEx.Server.Context.Model;

public class Appointment
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(MedicUser))]
    public int Medic { get; set; }

    [Required]
    [ForeignKey(nameof(PatientAppo))]
    public string? Patient { get; set; }

    [Required]
    public DateTime DateAppo { get; set; }

    [Required]
    public string? Desc { get; set; }

    [ForeignKey(nameof(LastAppointiment))]
    public int? LastAppo { get; set; }


    public User? MedicUser { get; set; }

    public Patient? PatientAppo { get; set; } 
    
    public Appointment? LastAppointiment { get; set; }
}