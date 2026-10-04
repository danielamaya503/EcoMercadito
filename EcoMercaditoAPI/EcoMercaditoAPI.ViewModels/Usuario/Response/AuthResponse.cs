using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.ViewModels.Usuario.Response;

/// <summary>
/// Respuesta de autenticación exitosa (login).
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Identificador único del usuario.
    /// </summary>
    public int UsuarioId { get; set; }

    /// <summary>
    /// Correo electrónico del usuario.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del usuario.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Número de teléfono del usuario.
    /// </summary>
    public string? Telefono { get; set; }

    /// <summary>
    /// Nombre del rol asignado.
    /// </summary>
    public string NombreRol { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del rol.
    /// </summary>
    public int RolId { get; set; }

    /// <summary>
    /// Identificador del municipio.
    /// </summary>
    public int MunicipioId { get; set; }

    /// <summary>
    /// Nombre del municipio (opcional).
    /// </summary>
    public string? MunicipioNombre { get; set; }

    /// <summary>
    /// Estado del usuario.
    /// </summary>
    public string Estado { get; set; } = string.Empty;

    /// <summary>
    /// Token de sesión (sin JWT, puede ser un GUID o hash).
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de expiración del token.
    /// </summary>
    public DateTime Expiracion { get; set; }
}
