using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Models.EcoMercadito
{
    /// <summary>
    /// Representa un departamento de El Salvador.
    /// </summary>
    /// 
    public class Departamento
    {
        /// <summary>
        /// Identificador único del departamento.
        /// </summary>
        public int DepartamentoId { get; set; }

        /// <summary>
        /// Nombre del departamento.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Municipios pertenecientes a este departamento.
        /// </summary>
        public ICollection<Municipio> Municipios { get; set; } = new List<Municipio>();
    }
}
