using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Models.Usuarios
{
    // <summary>
    /// Representa las credenciales de autenticación de un usuario.
    /// </summary>
    public class Credencial
    {
        /// <summary>
        /// Identificador único de la credencial.
        /// </summary>
        public int CredencialId { get; set; }

        /// <summary>
        /// Identificador del usuario propietario.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Hash de la contraseña (BCrypt).
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Proveedor de autenticación (Local, Google, Apple).
        /// </summary>
        public string ProveedorAuth { get; set; } = "Local";

        /// <summary>
        /// ID del usuario en el proveedor externo (si aplica).
        /// </summary>
        public string? ProveedorUserId { get; set; }

        /// <summary>
        /// Fecha de última actualización de la credencial.
        /// </summary>
        public DateTime FechaActualizacion { get; set; }

        /// <summary>
        /// Usuario propietario de esta credencial.
        /// </summary>
        public Usuario? Usuario { get; set; }
    }
}
