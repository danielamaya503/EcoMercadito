using EcoMercaditoAPI.ViewModels.EcoMercadito;
using EcoMercaditoAPI.ViewModels.EcoMercadito.Response;
using EcoMercaditoAPI.ViewModels.Helpers;

namespace EcoMercaditoAPI.Interfaces.EcoMercadito;

public interface INegocioService
{
    Task<ApiResponse<NegocioResponse>> CrearAsync(int usuarioLogueadoId,CrearNegocioRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<NegocioResponse>> ObtenerPorIdAsync(int negocioId,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<List<NegocioResponse>>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

    Task<ApiResponse<NegocioResponse>> ActualizarAsync(int negocioId, int usuarioLogueadoId, ActualizarNegocioRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<bool>> CambiarEstadoAsync(int negocioId, int usuarioLogueadoId,CambiarEstadoNegocioRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<NegocioResponse>> ObtenerPorUsuarioAsync(
        int usuarioId,
        CancellationToken cancellationToken = default);
}