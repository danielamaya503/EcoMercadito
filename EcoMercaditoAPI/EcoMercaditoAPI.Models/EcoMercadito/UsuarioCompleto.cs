using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Models.EcoMercadito
{
    /// <summary>
    /// Representa la vista vw_Usuario_Completo para consultas de usuarios con datos completos.
    /// Esta clase es de solo lectura, no se usa para inserciones/actualizaciones.
    /// </summary>
    public class UsuarioCompleto
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Número de teléfono del usuario.
        /// </summary>
        public string? Telefono { get; set; }

        /// <summary>
        /// Nombre del municipio donde reside el usuario.
        /// </summary>
        public string Municipio { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del departamento donde reside el usuario.
        /// </summary>
        public string Departamento { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del rol asignado al usuario.
        /// </summary>
        public string Rol { get; set; } = string.Empty;

        /// <summary>
        /// Estado del usuario (Activo, Inactivo, Suspendido).
        /// </summary>
        public string Estado { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de registro del usuario.
        /// </summary>
        public DateTime FechaRegistro { get; set; }
    }
}
