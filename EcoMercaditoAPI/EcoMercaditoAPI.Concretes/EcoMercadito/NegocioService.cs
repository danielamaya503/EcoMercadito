using EcoMercaditoAPI.Concretes.Context;
using EcoMercaditoAPI.Interfaces.EcoMercadito;
using EcoMercaditoAPI.Models.EcoMercadito;
using EcoMercaditoAPI.ViewModels.EcoMercadito;
using EcoMercaditoAPI.ViewModels.EcoMercadito.Response;
using EcoMercaditoAPI.ViewModels.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EcoMercaditoAPI.Concretes.EcoMercadito;

public class NegocioService: INegocioService
{
    private readonly AppDbContext _context;
    private readonly ILogger<NegocioService> _logger;

    public NegocioService(AppDbContext context, ILogger<NegocioService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<NegocioResponse>> CrearAsync(  int usuarioLogueadoId, CrearNegocioRequest request,CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    u => u.UsuarioId == usuarioLogueadoId && u.Estado == "Activo",
                    cancellationToken);
            
            if (usuario is null)
            {
                return ApiResponse<NegocioResponse>.CreateError(
                    "Usuario inválido",
                    ["El usuario no existe o está inactivo."],
                    StatusCodes.Status400BadRequest);
            }
            
            var municipioExiste = await _context.Municipios
                .AsNoTracking()
                .AnyAsync(m => m.MunicipioId == request.MunicipioId,
                    cancellationToken);
            
            if (!municipioExiste)
            {
                return ApiResponse<NegocioResponse>.CreateError(
                    "Municipio inválido",
                    ["El municipio no existe."],
                    StatusCodes.Status400BadRequest);
            }
            
            var usuarioTieneNegocio = await _context.Negocios
                .AsNoTracking()
                .AnyAsync(n => n.UsuarioId == usuarioLogueadoId &&
                               n.Estado != "Eliminado",
                    cancellationToken);
            
            if (usuarioTieneNegocio)
            {
                return ApiResponse<NegocioResponse>.CreateError(
                    "Negocio ya registrado",
                    ["El usuario ya tiene un negocio registrado."],
                    StatusCodes.Status409Conflict);
            }
            
            var nuevoNegocio = new Negocio
            {
                UsuarioId = usuarioLogueadoId,
                MunicipioId = request.MunicipioId,
                NombreComercial = request.NombreComercial.Trim(),
                Descripcion = string.IsNullOrWhiteSpace(request.Descripcion)
                    ? null
                    : request.Descripcion.Trim(),
                Estado = "Activo",
                FechaCreacion = DateTime.Now
            };
            
            _context.Negocios.Add(nuevoNegocio);
            await _context.SaveChangesAsync(cancellationToken);
            
            var response = await ObtenerNegocioInternoAsync(
                nuevoNegocio.NegocioId,
                cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
            
            return ApiResponse<NegocioResponse>.CreateSuccess(
                response!,
                "Negocio creado exitosamente",
                StatusCodes.Status201Created);
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError(
                ex,
                "Error de base de datos al crear negocio para el usuario {UsuarioId}",
                usuarioLogueadoId);

            return ApiResponse<NegocioResponse>.CreateError(
                "No se pudo crear el negocio",
                ["Ocurrió un error al guardar la información en la base de datos."],
                StatusCodes.Status500InternalServerError);
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError(
                ex,
                "Error inesperado al crear negocio para el usuario {UsuarioId}",
                usuarioLogueadoId);

            return ApiResponse<NegocioResponse>.CreateError(
                "Error al crear el negocio",
                ["Ocurrió un error inesperado. Intente más tarde."],
                StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ApiResponse<NegocioResponse>> ObtenerPorIdAsync(int negocioId, CancellationToken cancellationToken = default)
    {
        try
        {
            var negocio = await ObtenerNegocioInternoAsync(
            negocioId,
            cancellationToken);
            
            if (negocio is null)
            {
                return ApiResponse<NegocioResponse>.CreateError(
                    "Negocio no encontrado",
                    ["El negocio solicitado no existe."],
                    StatusCodes.Status404NotFound);
            }
            
            return ApiResponse<NegocioResponse>.CreateSuccess(
                negocio,
                "Negocio obtenido correctamente",
                StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error al obtener el negocio {NegocioId}",
                negocioId);

            return ApiResponse<NegocioResponse>.CreateError(
                "Error al obtener el negocio",
                ["Ocurrió un error inesperado. Intente más tarde."],
                StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ApiResponse<List<NegocioResponse>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var negocios = await _context.Negocios
                .AsNoTracking()
                .OrderByDescending(n => n.FechaCreacion)
                .Select(n => new NegocioResponse
                {
                    NegocioId = n.NegocioId,
                    UsuarioId = n.UsuarioId,
                    NombreUsuario = n.Usuario.Nombre,
                    NombreComercial = n.NombreComercial,
                    Descripcion = n.Descripcion,
                    MunicipioId = n.MunicipioId,
                    MunicipioNombre = n.Municipio.Nombre,
                    DepartamentoNombre = n.Municipio.Departamento.Nombre,
                    Estado = n.Estado,
                    FechaCreacion = n.FechaCreacion
                }) 
                .ToListAsync(cancellationToken);
            
            return ApiResponse<List<NegocioResponse>>.CreateSuccess(
                negocios,
                "Negocios obtenidos correctamente",
                StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error al obtener la lista de negocios");

            return ApiResponse<List<NegocioResponse>>.CreateError(
                "Error al obtener los negocios",
                ["Ocurrió un error inesperado. Intente más tarde."],
                StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ApiResponse<NegocioResponse>> ObtenerPorUsuarioAsync(int usuarioId, CancellationToken cancellationToken = default)
    {
        try
        {
            var negocio = await _context.Negocios
                .AsNoTracking()
                .Where(n => n.UsuarioId == usuarioId)
                .Select(n => new NegocioResponse
                {
                    NegocioId = n.NegocioId,
                    UsuarioId = n.UsuarioId,
                    NombreUsuario = n.Usuario.Nombre,
                    NombreComercial = n.NombreComercial,
                    Descripcion = n.Descripcion,
                    MunicipioId = n.MunicipioId,
                    MunicipioNombre = n.Municipio.Nombre,
                    DepartamentoNombre = n.Municipio.Departamento.Nombre,
                    Estado = n.Estado,
                    FechaCreacion = n.FechaCreacion
                })
                .FirstOrDefaultAsync(cancellationToken);
            
            if (negocio is null)
            {
                return ApiResponse<NegocioResponse>.CreateError(
                    "Negocio no encontrado",
                    ["El usuario no tiene un negocio registrado."],
                    StatusCodes.Status404NotFound);
            }
            
            return ApiResponse<NegocioResponse>.CreateSuccess(
                negocio,
                "Negocio obtenido correctamente",
                StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error al obtener el negocio del usuario {UsuarioId}",
                usuarioId);

            return ApiResponse<NegocioResponse>.CreateError(
                "Error al obtener el negocio",
                ["Ocurrió un error inesperado. Intente más tarde."],
                StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ApiResponse<NegocioResponse>> ActualizarAsync(int negocioId, int usuarioLogueadoId,ActualizarNegocioRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var usuario = await _context.Usuarios.AnyAsync(u => u.UsuarioId == usuarioLogueadoId);

            if (!usuario)
            {
                return ApiResponse<NegocioResponse>.CreateError(
                    "Acceso denegado",
                    ["No tienes permiso para actualizar este negocio."],
                    StatusCodes.Status403Forbidden);
            }

            var negocio = await _context.Negocios
                .FirstOrDefaultAsync(
                    n => n.NegocioId == negocioId && n.UsuarioId == usuarioLogueadoId,
                    cancellationToken);
            
            if (negocio is null)
            {
                return ApiResponse<NegocioResponse>.CreateError(
                    "Negocio no encontrado",
                    ["El negocio solicitado no existe."],
                    StatusCodes.Status404NotFound);
            }
            
            if (request.MunicipioId.HasValue)
            {
                var municipioExiste = await _context.Municipios
                    .AsNoTracking()
                    .AnyAsync(
                        m => m.MunicipioId == request.MunicipioId.Value,
                        cancellationToken);

                if (!municipioExiste)
                {
                    return ApiResponse<NegocioResponse>.CreateError(
                        "Municipio inválido",
                        ["El municipio no existe."],
                        StatusCodes.Status400BadRequest);
                }

                negocio.MunicipioId = request.MunicipioId.Value;
            }
            
            if (!string.IsNullOrWhiteSpace(request.NombreComercial))
            {
                negocio.NombreComercial = request.NombreComercial.Trim();
            }

            if (request.Descripcion is not null)
            {
                negocio.Descripcion = string.IsNullOrWhiteSpace(request.Descripcion)
                    ? null
                    : request.Descripcion.Trim();
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            
            var response = await ObtenerNegocioInternoAsync(
                negocioId,
                cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
            
            return ApiResponse<NegocioResponse>.CreateSuccess(
                response!,
                "Negocio actualizado exitosamente",
                StatusCodes.Status200OK);
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError(
                ex,
                "Error de base de datos al actualizar el negocio {NegocioId}",
                negocioId);

            return ApiResponse<NegocioResponse>.CreateError(
                "No se pudo actualizar el negocio",
                ["Ocurrió un error al guardar la información en la base de datos."],
                StatusCodes.Status500InternalServerError);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError(
                ex,
                "Error inesperado al actualizar el negocio {NegocioId}",
                negocioId);

            return ApiResponse<NegocioResponse>.CreateError(
                "Error al actualizar el negocio",
                ["Ocurrió un error inesperado. Intente más tarde."],
                StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ApiResponse<bool>> CambiarEstadoAsync(int negocioId, int usuarioLogueadoId, CambiarEstadoNegocioRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var usuario = await _context.Usuarios.AnyAsync(u => u.UsuarioId == usuarioLogueadoId);

            if (!usuario)
            {
                return ApiResponse<bool>.CreateError(
                    "Acceso denegado",
                    ["No tienes permiso para actualizar este negocio."],
                    StatusCodes.Status403Forbidden);
            }
            
            var negocio = await _context.Negocios
                .FirstOrDefaultAsync(
                    n => n.NegocioId == negocioId && n.UsuarioId == usuarioLogueadoId,
                    cancellationToken);

            if (negocio is null)
            {
                return ApiResponse<bool>.CreateError(
                    "Negocio no encontrado",
                    ["El negocio solicitado no existe."],
                    StatusCodes.Status404NotFound);
            }
            
            negocio.Estado = request.Estado.Trim();

            if (negocio.Estado == "Activo" || negocio.Estado == "Inactivo")
            {
                return ApiResponse<bool>.CreateError(
                    "Estado invalido",
                    ["El estado enviado es invalido."],
                    StatusCodes.Status400BadRequest);
            }

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            return ApiResponse<bool>.CreateSuccess(
                true,
                $"Estado del negocio actualizado a {negocio.Estado}.",
                StatusCodes.Status200OK);
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError(
                ex,
                "Error de base de datos al cambiar estado del negocio {NegocioId}",
                negocioId);

            return ApiResponse<bool>.CreateError(
                "No se pudo cambiar el estado del negocio",
                ["Ocurrió un error al guardar la información en la base de datos."],
                StatusCodes.Status500InternalServerError);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError(
                ex,
                "Error inesperado al cambiar estado del negocio {NegocioId}",
                negocioId);

            return ApiResponse<bool>.CreateError(
                "Error al cambiar el estado del negocio",
                ["Ocurrió un error inesperado. Intente más tarde."],
                StatusCodes.Status500InternalServerError);
        }
    }
    
    
    //Private metho
    
    private async Task<NegocioResponse?> ObtenerNegocioInternoAsync(int negocioId,
        CancellationToken cancellationToken)
    {
        return await _context.Negocios
            .AsNoTracking()
            .Where(n => n.NegocioId == negocioId)
            .Select(n => new NegocioResponse
            {
                NegocioId = n.NegocioId,
                UsuarioId = n.UsuarioId,
                NombreUsuario = n.Usuario.Nombre,
                NombreComercial = n.NombreComercial,
                Descripcion = n.Descripcion,
                MunicipioId = n.MunicipioId,
                MunicipioNombre = n.Municipio.Nombre,
                DepartamentoNombre = n.Municipio.Departamento.Nombre,
                Estado = n.Estado,
                FechaCreacion = n.FechaCreacion
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}