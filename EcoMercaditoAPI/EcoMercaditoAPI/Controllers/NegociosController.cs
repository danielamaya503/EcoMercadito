using EcoMercaditoAPI.Interfaces.EcoMercadito;
using EcoMercaditoAPI.ViewModels.EcoMercadito;
using EcoMercaditoAPI.ViewModels.EcoMercadito.Response;
using EcoMercaditoAPI.ViewModels.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoMercaditoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NegociosController: ControllerBase
{
   private readonly INegocioService _negocioService;

   public NegociosController(INegocioService negocioService)
   {
      _negocioService = negocioService;
   }

   /// <summary>
   /// Obtiene todos los negocios registrados.
   /// </summary>
   [HttpGet]
   [ProducesResponseType(typeof(ApiResponse<List<NegocioResponse>>), StatusCodes.Status200OK)]
   [ProducesResponseType(typeof(ApiResponse<List<NegocioResponse>>), StatusCodes.Status401Unauthorized)]
   [ProducesResponseType(typeof(ApiResponse<List<NegocioResponse>>), StatusCodes.Status403Forbidden)]
   [ProducesResponseType(typeof(ApiResponse<List<NegocioResponse>>), StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<ApiResponse<List<NegocioResponse>>>> GetAll()
   {
      var result = await _negocioService.ObtenerTodosAsync();
      return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
   }
   
   /// <summary>
   /// Obtiene un negocio por ID.
   /// Cualquier usuario autenticado puede consultarlo.
   /// </summary>
   [HttpGet("{negocioId:int}")]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status200OK)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status401Unauthorized)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status404NotFound)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<ApiResponse<NegocioResponse>>> GetById(int negocioId)
   {
      var result = await _negocioService.ObtenerPorIdAsync(negocioId);
      return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
   }
   
   /// <summary>
   /// Obtiene el negocio del usuario logueado.
   /// </summary>
   [HttpGet("mi-negocio")]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status200OK)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status401Unauthorized)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status404NotFound)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<ApiResponse<NegocioResponse>>> GetMyNegocio(
      [FromHeader(Name = "X-Usuario-Id")] int usuarioLogueadoId)
   {
      if (usuarioLogueadoId <= 0)
      {
         return BadRequest(ApiResponse<NegocioResponse>.CreateError(
            "Usuario no autenticado",
            new List<string> { "El header X-Usuario-Id es requerido" }
         ));
      }

      var result = await _negocioService.ObtenerPorUsuarioAsync(usuarioLogueadoId);
      return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
   }
   
   /// <summary>
   /// Registra el negocio del usuario logueado.
   /// Un usuario solo puede tener un negocio.
   /// </summary>
   [HttpPost]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status201Created)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status400BadRequest)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status409Conflict)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<ApiResponse<NegocioResponse>>> Create(
      [FromHeader(Name = "X-Usuario-Id")] int usuarioLogueadoId,
      [FromBody] CrearNegocioRequest request)
   {
      if (usuarioLogueadoId <= 0)
      {
         return BadRequest(ApiResponse<NegocioResponse>.CreateError(
            "Usuario no autenticado",
            new List<string> { "El header X-Usuario-Id es requerido" }
         ));
      }

      var result = await _negocioService.CrearAsync(usuarioLogueadoId,request);
      return result.Success
         ? StatusCode(StatusCodes.Status201Created, result)
         : StatusCode(result.StatusCode, result);
   }
   
   /// <summary>
   /// Actualiza parcialmente el negocio del usuario logueado.
   /// Solo se modifican los campos enviados.
   /// </summary>
   [HttpPut("{negocioId:int}")]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status200OK)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status400BadRequest)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status403Forbidden)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status404NotFound)]
   [ProducesResponseType(typeof(ApiResponse<NegocioResponse>), StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<ApiResponse<NegocioResponse>>> Update(
      [FromHeader(Name = "X-Usuario-Id")] int usuarioLogueadoId,
      int negocioId,
      [FromBody] ActualizarNegocioRequest request)
   {
      if (usuarioLogueadoId <= 0)
      {
         return BadRequest(ApiResponse<NegocioResponse>.CreateError(
            "Usuario no autenticado",
            new List<string> { "El header X-Usuario-Id es requerido" }
         ));
      }

      var negocioResult = await _negocioService.ObtenerPorIdAsync(negocioId);

      if (!negocioResult.Success)
      {
         return StatusCode(negocioResult.StatusCode, negocioResult);
      }

      if (negocioResult.Data!.UsuarioId != usuarioLogueadoId)
      {
         return StatusCode(StatusCodes.Status403Forbidden,
            ApiResponse<NegocioResponse>.CreateError(
               "Acceso denegado",
               new List<string> { "No tienes permiso para actualizar este negocio." },
               StatusCodes.Status403Forbidden
            ));
      }

      var result = await _negocioService.ActualizarAsync(negocioId, usuarioLogueadoId, request);
      return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
   }
   
   /// <summary>
   /// Cambia el estado del negocio del usuario logueado.
   /// </summary>
   [HttpPatch("{negocioId:int}/estado")]
   [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
   [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
   [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status403Forbidden)]
   [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
   [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<ApiResponse<bool>>> ChangeStatus(
      [FromHeader(Name = "X-Usuario-Id")] int usuarioLogueadoId,
      int negocioId,
      [FromBody] CambiarEstadoNegocioRequest request)
   {
      if (usuarioLogueadoId <= 0)
      {
         return BadRequest(ApiResponse<bool>.CreateError(
            "Usuario no autenticado",
            new List<string> { "El header X-Usuario-Id es requerido" }
         ));
      }

      var negocioResult = await _negocioService.ObtenerPorIdAsync(negocioId);

      if (!negocioResult.Success)
      {
         return StatusCode(negocioResult.StatusCode, negocioResult);
      }

      if (negocioResult.Data!.UsuarioId != usuarioLogueadoId)
      {
         return StatusCode(StatusCodes.Status403Forbidden,
            ApiResponse<bool>.CreateError(
               "Acceso denegado",
               new List<string> { "No tienes permiso para cambiar el estado de este negocio." },
               StatusCodes.Status403Forbidden
            ));
      }

      var result = await _negocioService.CambiarEstadoAsync(negocioId, usuarioLogueadoId, request);
      return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
   }
   

}