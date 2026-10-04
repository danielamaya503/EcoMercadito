using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcoMercaditoAPI.ViewModels.Usuario.Request;

/// <summary>
/// Request para autenticación de usuario (login).
/// </summary>
public class LoginRequest
{
    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    [MaxLength(255, ErrorMessage = "El correo electrónico no puede exceder los 255 caracteres")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MaxLength(255, ErrorMessage = "La contraseña no puede exceder los 255 caracteres")]
    public string Password { get; set; } = string.Empty;
}
