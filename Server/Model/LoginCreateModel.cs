using System.ComponentModel.DataAnnotations;

namespace ApiMedicalClinicEx.Server.Model;

public class LoginCreateModel
{
    [Required(ErrorMessage = "E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage ="Email inválido.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Nome inválido.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "Telefone é obrigatório.")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Senha é obrigatório.")]
    [StringLength(24, MinimumLength = 8, ErrorMessage ="Senha deve conter entre 8 e 24 caracteres.")]
    public string? Password { get; set; }

    [Compare(nameof(Password), ErrorMessage ="Confirmação de senha inválida.")]
    public string? ConfirmPassword { get; set; }
}