using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Models.Usuarios
{
    /// <summary>
    /// Representa un rol de usuario en el sistema.
    /// </summary>
    public class Rol
    {
        /// <summary>
        /// Identificador único del rol.
        /// </summary>
        public int RolId { get; set; }

        /// <summary>
        /// Nombre del rol (Comprador, Comercio, Administrador).
        /// </summary>
        public string NombreRol { get; set; } = string.Empty;

        /// <summary>
        /// Usuarios asignados a este rol.
        /// </summary>
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
