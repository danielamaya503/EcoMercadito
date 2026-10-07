using EcoMercaditoAPI.ViewModels.Helpers;
using EcoMercaditoAPI.ViewModels.Usuario.Request;
using EcoMercaditoAPI.ViewModels.Usuario.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Interfaces.Usuaio;

public interface IUsuarioService
{
    /// <summary>
    /// Obtiene todos los usuarios paginados.
    /// </summary>
    /// <param name="pageNumber">Número de página (1-based).</param>
    /// <param name="pageSize">Cantidad de elementos por página (1-100).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista paginada de usuarios.</returns>
    Task<ApiResponse<PagedResponse<UsuarioResponse>>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un usuario por su ID.
    /// </summary>
    /// <param name="usuarioId">Identificador del usuario.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Datos del usuario o error si no existe.</returns>
    Task<ApiResponse<UsuarioResponse>> GetByIdAsync(int usuarioId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea un nuevo usuario.
    /// Solo administradores pueden crear usuarios.
    /// </summary>
    /// <param name="request">Datos del nuevo usuario.</param>
    /// <param name="usuarioLogueadoId">ID del usuario que realiza la operación (debe ser administrador).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Datos del usuario creado.</returns>
    Task<ApiResponse<UsuarioResponse>> CreateAsync(CreateUsuarioRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Edita un usuario existente.
    /// Solo el mismo usuario o un administrador puede editar.
    /// </summary>
    /// <param name="request">Datos a actualizar.</param>
    /// <param name="usuarioLogueadoId">ID del usuario que realiza la operación.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Datos del usuario actualizado.</returns>
    Task<ApiResponse<UsuarioResponse>> UpdateAsync(UpdateUsuarioRequest request, int usuarioLogueadoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Desactiva un usuario.
    /// Solo administradores pueden desactivar usuarios.
    /// </summary>
    /// <param name="usuarioLogueadoId">ID del administrador que realiza la operación.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si se desactivó exitosamente.</returns>
    Task<ApiResponse<bool>> DesactivarAsync(int usuarioId, int usuarioLogueadoId, CancellationToken cancellationToken = default);
}
