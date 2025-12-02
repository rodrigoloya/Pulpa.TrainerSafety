using System.ComponentModel.DataAnnotations;

namespace Pulpa.TrainerSafety.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es requerido")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "La contraseña debe contener al menos una mayúscula, una minúscula, un número y un carácter especial")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe confirmar la contraseña")]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe aceptar los términos y condiciones")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Debe aceptar los términos y condiciones")]
    public bool AcceptTerms { get; set; }

    public bool ReceiveNewsletters { get; set; }
}

public class LoginViewModel
{
    [Required(ErrorMessage = "El correo electrónico es requerido")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "El correo electrónico es requerido")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    public string Email { get; set; } = string.Empty;
}
