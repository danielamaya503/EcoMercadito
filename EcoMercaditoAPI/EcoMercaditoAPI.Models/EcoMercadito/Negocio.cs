using EcoMercaditoAPI.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Models.EcoMercadito
{
    /// <summary>
    /// Representa un negocio o comercio registrado.
    /// </summary>
    public class Negocio
    {
        /// <summary>
        /// Identificador único del negocio.
        /// </summary>
        public int NegocioId { get; set; }

        /// <summary>
        /// Identificador del usuario propietario.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Nombre comercial del negocio.
        /// </summary>
        public string NombreComercial { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del negocio.
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// Identificador del municipio donde está ubicado.
        /// </summary>
        public int MunicipioId { get; set; }

        /// <summary>
        /// Estado del negocio (Activo, Inactivo).
        /// </summary>
        public string Estado { get; set; } = "Activo";

        /// <summary>
        /// Fecha de creación del negocio.
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Usuario propietario de este negocio.
        /// </summary>
        public Usuario? Usuario { get; set; }

        /// <summary>
        /// Municipio donde está ubicado el negocio.
        /// </summary>
        public Municipio? Municipio { get; set; }

        /// <summary>
        /// Ofertas excedentes publicadas por este negocio.
        /// </summary>
        public ICollection<OfertaExcedente> OfertasExcedentes { get; set; } = new List<OfertaExcedente>();
    }
}
