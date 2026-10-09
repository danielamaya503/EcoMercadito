using EcoMercaditoAPI.Models.EcoMercadito;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EcoMercaditoAPI.Models.Usuarios;

/// <summary>
/// Representa un usuario del sistema (identidad y autenticación).
/// </summary>
public class Usuario
{
    /// <summary>
    /// Identificador único del usuario.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UsuarioId { get; set; }

    /// <summary>
    /// Nombre completo del usuario.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico único para login.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Número de teléfono opcional.
    /// </summary>
    public string? Telefono { get; set; }

    /// <summary>
    /// Identificador del municipio de residencia.
    /// </summary>
    public int MunicipioId { get; set; }

    /// <summary>
    /// Identificador del rol asignado.
    /// </summary>
    public int RolId { get; set; }

    /// <summary>
    /// Estado del usuario (Activo, Inactivo, Suspendido).
    /// </summary>
    public string Estado { get; set; } = "Activo";

    /// <summary>
    /// Fecha de registro del usuario.
    /// </summary>
    public DateTime FechaRegistro { get; set; }

    /// <summary>
    /// Fecha de baneo (si aplica).
    /// </summary>
    public DateTime? FechaBaneo { get; set; }

    /// <summary>
    /// Municipio de residencia del usuario.
    /// </summary>
    public Municipio? Municipio { get; set; }

    /// <summary>
    /// Rol asignado al usuario.
    /// </summary>
    public Rol? Rol { get; set; }

    /// <summary>
    /// Credencial de autenticación del usuario.
    /// </summary>
    public Credencial? Credencial { get; set; }

    /// <summary>
    /// Negocio asociado al usuario (si es comercio).
    /// </summary>
    public Negocio? Negocio { get; set; }
}
