using System.ComponentModel.DataAnnotations;

namespace ApiMedicalClinicEx.Server.Model;

public class LoginRegisterModel
{
    [Required(ErrorMessage ="Usuário obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha obrigatória.")]
    [StringLength(24, MinimumLength = 8, ErrorMessage = "Senha deve conter entre 8 e 24 caracteres.")]
    public string Password { get; set; } = string.Empty;
}