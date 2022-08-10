namespace ApiMedicalClinicEx.Server.Model;

public class CurrentlyUserModel
{
    public int UserId { get; set; }
    public string MedicalId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}