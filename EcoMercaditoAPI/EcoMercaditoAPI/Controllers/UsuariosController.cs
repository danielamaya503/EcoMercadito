using EcoMercaditoAPI.Interfaces.Usuaio;
using EcoMercaditoAPI.ViewModels.Helpers;
using EcoMercaditoAPI.ViewModels.Usuario.Request;
using EcoMercaditoAPI.ViewModels.Usuario.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoMercaditoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los usuarios paginados.
        /// Solo administradores.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<UsuarioResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<UsuarioResponse>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<UsuarioResponse>>), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<PagedResponse<UsuarioResponse>>>> GetAll(
            [FromHeader(Name = "X-Usuario-Id")] int usuarioLogueadoId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 1;
            if (pageSize > 100) pageSize = 100;

            var result = await _usuarioService.GetAllPagedAsync(pageNumber, pageSize);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene un usuario por ID.
        /// Solo administradores o el mismo usuario.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<UsuarioResponse>>> GetById(
            [FromHeader(Name = "X-Usuario-Id")] int usuarioLogueadoId,
            int id)
        {
            if (usuarioLogueadoId <= 0)
            {
                return BadRequest(ApiResponse<UsuarioResponse>.CreateError(
                    "Usuario no autenticado",
                    new List<string> { "El header X-Usuario-Id es requerido" }
                ));
            }

            var result = await _usuarioService.GetByIdAsync(id);
            return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UsuarioResponse>>> Create(
            [FromBody] CreateUsuarioRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<UsuarioResponse>.CreateError(
                    "Datos inválidos",
                    errors
                ));
            }

            var result = await _usuarioService.CreateAsync(request);
            return result.Success ? CreatedAtAction(nameof(GetById), new { id = result.Data.UsuarioId }, result) : StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Edita un usuario existente.
        /// Solo el mismo usuario o administradores.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UsuarioResponse>>> Update(
            [FromHeader(Name = "X-Usuario-Id")] int usuarioLogueadoId,
            int id,
            [FromBody] UpdateUsuarioRequest request)
        {
            if (usuarioLogueadoId <= 0)
            {
                return BadRequest(ApiResponse<UsuarioResponse>.CreateError(
                    "Usuario no autenticado",
                    new List<string> { "El header X-Usuario-Id es requerido" }
                ));
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<UsuarioResponse>.CreateError(
                    "Datos inválidos",
                    errors
                ));
            }

            if (request.UsuarioId != id)
            {
                return BadRequest(ApiResponse<UsuarioResponse>.CreateError(
                    "ID inválido",
                    new List<string> { "El ID en la ruta no coincide con el ID en el body" }
                ));
            }

            var result = await _usuarioService.UpdateAsync(request, usuarioLogueadoId);
            return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Desactiva un usuario.
        /// Solo administradores.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> Desactivar(
            [FromHeader(Name = "X-Usuario-Id")] int usuarioLogueadoId,
            int id)
        {
            if (usuarioLogueadoId <= 0)
            {
                return BadRequest(ApiResponse<bool>.CreateError(
                    "Usuario no autenticado",
                    new List<string> { "El header X-Usuario-Id es requerido" }
                ));
            }

            var result = await _usuarioService.DesactivarAsync(id, usuarioLogueadoId);
            return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);
        }


    }

}
