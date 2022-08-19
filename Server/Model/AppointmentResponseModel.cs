using System.ComponentModel.DataAnnotations;

namespace ApiMedicalClinicEx.Server.Model;

/// <summary>
/// use if appointment id is required
/// </summary>
public class AppointmentResponseModel : AppointmentModel
{
    [Required]
    public int Id { get; set; }
}