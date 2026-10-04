using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcoMercaditoAPI.ViewModels.Usuario.Request;

public class CreateUsuarioRequest
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    [MaxLength(255, ErrorMessage = "El correo electrónico no puede exceder los 255 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "El formato del teléfono no es válido")]
    [MaxLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "El municipio es obligatorio")]
    [Range(1, 262, ErrorMessage = "El municipio debe ser un ID válido (1-262)")]
    public int MunicipioId { get; set; }

    [Required(ErrorMessage = "El rol es obligatorio")]
    [Range(1, 3, ErrorMessage = "El rol debe ser 1 (Comprador), 2 (Comercio) o 3 (Administrador)")]
    public int RolId { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    [MaxLength(250, ErrorMessage = "La contraseña no puede exceder los 250 caracteres")]
    public string Password { get; set; } = string.Empty;
}
