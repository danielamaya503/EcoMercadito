using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Models.EcoMercadito
{
    /// <summary>
    /// Representa la vista vw_Oferta_Completa para consultas de ofertas con datos completos.
    /// Esta clase es de solo lectura, no se usa para inserciones/actualizaciones.
    /// </summary>
    public class OfertaCompleta
    {
        /// <summary>
        /// Identificador único de la oferta.
        /// </summary>
        public int OfertaId { get; set; }

        /// <summary>
        /// Título de la oferta.
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
        public bool Disponible { get; set; }

        /// <summary>
        /// Nombre de la categoría del producto.
        /// </summary>
        public string NombreCategoria { get; set; } = string.Empty;

        /// <summary>
        /// Nombre comercial del negocio que publica la oferta.
        /// </summary>
        public string NombreComercial { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de la persona de contacto del negocio.
        /// </summary>
        public string NombreContacto { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono de contacto del negocio.
        /// </summary>
        public string? TelefonoContacto { get; set; }

        /// <summary>
        /// Nombre del municipio donde está ubicado el negocio.
        /// </summary>
        public string Municipio { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del departamento donde está ubicado el negocio.
        /// </summary>
        public string Departamento { get; set; } = string.Empty;
    }
}
