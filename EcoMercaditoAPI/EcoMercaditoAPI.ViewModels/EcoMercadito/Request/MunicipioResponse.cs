using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.ViewModels.EcoMercadito.Request;

/// <summary>
/// DTO para respuesta de municipio sin relaciones cíclicas.
/// </summary>
public class MunicipioResponse
{
    /// <summary>
    /// Identificador único del municipio.
    /// </summary>
    public int MunicipioId { get; set; }

    /// <summary>
    /// Nombre del municipio.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del departamento.
    /// </summary>
    public int DepartamentoId { get; set; }

    /// <summary>
    /// Nombre del departamento (solo lectura).
    /// </summary>
    public string? DepartamentoNombre { get; set; }
}

/// <summary>
/// DTO para respuesta detallada de municipio.
/// </summary>
public class MunicipioDetalleResponse
{
    /// <summary>
    /// Identificador único del municipio.
    /// </summary>
    public int MunicipioId { get; set; }

    /// <summary>
    /// Nombre del municipio.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del departamento.
    /// </summary>
    public int DepartamentoId { get; set; }

    /// <summary>
    /// Información del departamento (sin ciclo).
    /// </summary>
    public DepartamentoSimpleResponse? Departamento { get; set; }
}


/// <summary>
/// DTO simple para departamento
/// </summary>
public class DepartamentoSimpleResponse
{
    /// <summary>
    /// Identificador del departamento.
    /// </summary>
    public int DepartamentoId { get; set; }

    /// <summary>
    /// Nombre del departamento.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;
}