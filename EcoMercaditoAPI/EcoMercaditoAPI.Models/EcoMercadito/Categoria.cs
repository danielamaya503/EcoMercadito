using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Models.EcoMercadito
{
    /// <summary>
    /// Representa una categoría de productos.
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// Identificador único de la categoría.
        /// </summary>
        public int CategoriaId { get; set; }

        /// <summary>
        /// Nombre de la categoría.
        /// </summary>
        public string NombreCategoria { get; set; } = string.Empty;

        /// <summary>
        /// Clase CSS del ícono de la categoría.
        /// </summary>
        public string? IconoClase { get; set; }

        /// <summary>
        /// Indica si la categoría está activa.
        /// </summary>
        public bool Estado { get; set; } = true;

        /// <summary>
        /// Ofertas excedentes en esta categoría.
        /// </summary>
        public ICollection<OfertaExcedente> OfertasExcedentes { get; set; } = new List<OfertaExcedente>();
    }
}
