using EcoMercaditoAPI.Interfaces.EcoMercadito;
using EcoMercaditoAPI.Models.EcoMercadito;
using EcoMercaditoAPI.ViewModels.EcoMercadito.Request;
using EcoMercaditoAPI.ViewModels.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoMercaditoAPI.Controllers;

/// <summary>
/// Controlador para operaciones de municipios.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MunicipiosController : ControllerBase
{
    private readonly IMunicipioService _municipioService;
    private readonly ILogger<MunicipiosController> _logger;

    public MunicipiosController(IMunicipioService municipioService, ILogger<MunicipiosController> logger)
    {
        _municipioService = municipioService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los municipios.
    /// </summary>
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<MunicipioResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<MunicipioResponse>>), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<MunicipioResponse>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        // Validar parámetros
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 1;
        if (pageSize > 100) pageSize = 100;

        var result = await _municipioService.GetAllPagedAsync(pageNumber, pageSize);

        return Ok(ApiResponse<PagedResponse<MunicipioResponse>>.CreateSuccess(
            result,
            $"Se obtuvieron {result.Data.Count} municipios de la página {result.CurrentPage}"
        ));
    }

    /// <summary>
    /// Obtiene un municipio por su ID.
    /// </summary>
    [ProducesResponseType(typeof(ApiResponse<MunicipioDetalleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MunicipioDetalleResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<MunicipioDetalleResponse>), StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}", Name = "GetMunicipioById")]
    public async Task<ActionResult<ApiResponse<Municipio>>> GetById(int id)
    {
        var municipio = await _municipioService.GetByIdAsync(id);

        if (municipio == null)
        {
            _logger.LogWarning("Municipio con ID {MunicipioId} no encontrado", id);
            return NotFound(ApiResponse<MunicipioDetalleResponse>.CreateError(
                $"El municipio con ID {id} no existe"
            ));
        }

        return Ok(ApiResponse<MunicipioDetalleResponse>.CreateSuccess(
           municipio,
           "Municipio obtenido exitosamente"
       ));
    }

    /// <summary>
    /// Obtiene municipios por departamento paginados.
    /// </summary>
    /// <param name="departamentoId">ID del departamento.</param>
    /// <param name="pageNumber">Número de página (default: 1).</param>
    /// <param name="pageSize">Cantidad por página (default: 20, max: 100).</param>
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<MunicipioResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<MunicipioResponse>>), StatusCodes.Status500InternalServerError)]
    [HttpGet("departamento/{departamentoId}", Name = " ")]
    public async Task<ActionResult<ApiResponse<PagedResponse<MunicipioResponse>>>> GetByDepartamento(
        int departamentoId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 1;
        if (pageSize > 100) pageSize = 100;

        var result = await _municipioService.GetByDepartamentoIdPagedAsync(departamentoId, pageNumber, pageSize);

        return Ok(ApiResponse<PagedResponse<MunicipioResponse>>.CreateSuccess(
            result,
            $"Se obtuvieron {result.Data.Count} municipios del departamento"
        ));
    }

    /// <summary>
    /// Busca municipios por nombre paginados.
    /// </summary>
    /// <param name="nombre">Texto a buscar en el nombre.</param>
    /// <param name="pageNumber">Número de página (default: 1).</param>
    /// <param name="pageSize">Cantidad por página (default: 20, max: 100).</param>
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<MunicipioResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<MunicipioResponse>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<MunicipioResponse>>), StatusCodes.Status500InternalServerError)]
    [HttpGet("buscar", Name = "SearchMunicipios")]
    public async Task<ActionResult<ApiResponse<PagedResponse<MunicipioResponse>>>> Search(
        [FromQuery] string nombre,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return BadRequest(ApiResponse<List<MunicipioResponse>>.CreateError(
                "El parámetro 'nombre' es requerido"
            ));
        }

        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 1;
        if (pageSize > 100) pageSize = 100;

        var result = await _municipioService.SearchByNamePagedAsync(nombre, pageNumber, pageSize);

        return Ok(ApiResponse<PagedResponse<MunicipioResponse>>.CreateSuccess(
            result,
            $"Se encontraron {result.TotalCount} municipios que coinciden con '{nombre}'"
        ));
    }
}
