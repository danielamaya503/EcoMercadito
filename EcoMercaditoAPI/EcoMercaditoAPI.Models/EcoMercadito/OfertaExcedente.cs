using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Models.EcoMercadito
{
    /// <summary>
    /// Representa una oferta de producto excedente publicada por un negocio.
    /// </summary>
    public class OfertaExcedente
    {
        /// <summary>
        /// Identificador único de la oferta.
        /// </summary>
        public int OfertaId { get; set; }

        /// <summary>
        /// Identificador del negocio que publica la oferta.
        /// </summary>
        public int NegocioId { get; set; }

        /// <summary>
        /// Identificador de la categoría del producto.
        /// </summary>
        public int CategoriaId { get; set; }

        /// <summary>
        /// Título descriptivo de la oferta.
        /// </summary>
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Precio original del producto.
        /// </summary>
        public decimal PrecioOriginal { get; set; }

        /// <summary>
        /// Precio con descuento aplicado.
        /// </summary>
        public decimal PrecioDescuento { get; set; }

        /// <summary>
        /// Cantidad disponible del producto.
        /// </summary>
        public int CantidadDisponible { get; set; }

        /// <summary>
        /// Fecha límite de vigencia de la oferta.
        /// </summary>
        public DateTime FechaLimite { get; set; }

        /// <summary>
        /// Indica si la oferta está disponible.
        /// </summary>
        public bool Disponible { get; set; } = true;

        /// <summary>
        /// Fecha de creación de la oferta.
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Negocio que publica esta oferta.
        /// </summary>
        public Negocio? Negocio { get; set; }

        /// <summary>
        /// Categoría del producto ofertado.
        /// </summary>
        public Categoria? Categoria { get; set; }
    }
}
