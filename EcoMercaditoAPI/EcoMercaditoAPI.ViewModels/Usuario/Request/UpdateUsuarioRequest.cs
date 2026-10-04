using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcoMercaditoAPI.ViewModels.Usuario.Request;

public class UpdateUsuarioRequest
{
    [Required]
    public int UsuarioId { get; set; }

    [MaxLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres")]
    public string? Nombre { get; set; }

    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    [MaxLength(255, ErrorMessage = "El correo electrónico no puede exceder los 255 caracteres")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "El formato del teléfono no es válido")]
    [MaxLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
    public string? Telefono { get; set; }

    [Range(1, 262, ErrorMessage = "El municipio debe ser un ID válido (1-262)")]
    public int? MunicipioId { get; set; }

    [Range(1, 3, ErrorMessage = "El rol debe ser 1, 2 o 3")]
    public int? RolId { get; set; }

    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    [MaxLength(250, ErrorMessage = "La contraseña no puede exceder los 250 caracteres")]
    public string? Password { get; set; }
}
