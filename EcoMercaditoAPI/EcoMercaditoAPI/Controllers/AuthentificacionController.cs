using EcoMercaditoAPI.Interfaces.Usuaio;
using EcoMercaditoAPI.ViewModels.Helpers;
using EcoMercaditoAPI.ViewModels.Usuario.Request;
using EcoMercaditoAPI.ViewModels.Usuario.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoMercaditoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthentificacionController : ControllerBase
{
    private readonly IAuthentificacion _authService;
    private readonly ILogger<AuthentificacionController> _logger;

    public AuthentificacionController(IAuthentificacion authService, ILogger<AuthentificacionController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Autentica un usuario con email y contraseña.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<AuthResponse>.CreateError(
                "Datos inválidos",
                errors,
                StatusCodes.Status400BadRequest
            ));
        }

        var result = await _authService.LoginAsync(request);
        return result.Success ? Ok(result) : StatusCode(result.StatusCode, result);

    }
}
