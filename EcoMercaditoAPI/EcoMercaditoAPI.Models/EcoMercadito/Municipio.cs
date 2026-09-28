using EcoMercaditoAPI.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Models.EcoMercadito
{
    /// <summary>
    /// Representa un municipio de El Salvador.
    /// </summary>
    public class Municipio
    {
        /// <summary>
        /// Identificador único del municipio.
        /// </summary>
        public int MunicipioId { get; set; }

        /// <summary>
        /// Identificador del departamento al que pertenece.
        /// </summary>
        public int DepartamentoId { get; set; }

        /// <summary>
        /// Nombre del municipio.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Departamento al que pertenece este municipio.
        /// </summary>
        public Departamento? Departamento { get; set; }

        /// <summary>
        /// Usuarios registrados en este municipio.
        /// </summary>
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

        /// <summary>
        /// Negocios ubicados en este municipio.
        /// </summary>
        public ICollection<Negocio> Negocios { get; set; } = new List<Negocio>();
    }
}
