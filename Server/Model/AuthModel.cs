namespace ApiMedicalClinicEx.Server.Model;

public class AuthModel
{
    public string Token { get; set; } = string.Empty;
    public DateTime Exp { get; set; }
    public UserModel User  { get; set; } = new();
}