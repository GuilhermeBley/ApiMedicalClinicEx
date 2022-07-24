using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMedicalClinicEx.Server.Context.Model;

/// <summary>
/// Principal Users
/// </summary>
public class User : IdentityUser<int>
{
    [Required, Column(TypeName = "varchar(100)")]
    public string IdDoctor { get; set; } = string.Empty;
}
