using EcoMercaditoAPI.Concretes.Context;
using EcoMercaditoAPI.Interfaces.Usuaio;
using EcoMercaditoAPI.Models.EcoMercadito;
using EcoMercaditoAPI.Models.Usuarios;
using EcoMercaditoAPI.ViewModels.EcoMercadito.Request;
using EcoMercaditoAPI.ViewModels.Helpers;
using EcoMercaditoAPI.ViewModels.Usuario.Request;
using EcoMercaditoAPI.ViewModels.Usuario.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EcoMercaditoAPI.Concretes.Usuario;

public class UsuarioService: IUsuarioService
{
    private readonly AppDbContext _context;
    private readonly ILogger<UsuarioService> _logger;

    public UsuarioService(AppDbContext context, ILogger<UsuarioService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<PagedResponse<UsuarioResponse>>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        try 
        {
            var query = _context.Usuarios
                           .Include(u => u.Rol)
                           .Include(u => u.Municipio)
                           .OrderBy(u => u.Nombre)
                           .AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var usuarios = await _context.Usuarios
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var response = usuarios.Select(u => new UsuarioResponse
            {
                UsuarioId = u.UsuarioId,
                Nombre = u.Nombre,
                Email = u.Email,
                Telefono = u.Telefono,
                MunicipioId = u.MunicipioId,
                MunicipioNombre = u.Municipio?.Nombre,
                RolId = u.RolId,
                NombreRol = u.Rol?.NombreRol ?? "Sin rol",
                Estado = u.Estado,
                FechaRegistro = u.FechaRegistro,
                FechaBaneo = u.FechaBaneo
            }).ToList();

            var pagedResponse = PagedResponse<UsuarioResponse>.Create(response, pageNumber, pageSize, totalCount);

            return ApiResponse<PagedResponse<UsuarioResponse>>.CreateSuccess(
                 pagedResponse,
                 $"Se obtuvieron {pagedResponse.Data.Count} usuarios"
             );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuarios paginados");
            return ApiResponse<PagedResponse<UsuarioResponse>>.CreateError(
                "Error al obtener los usuarios",
                new List<string> { ex.Message },
                StatusCodes.Status500InternalServerError
            );
        }

    }

    public async Task<ApiResponse<UsuarioResponse>> GetByIdAsync(int usuarioId, CancellationToken cancellationToken = default)
    {
        try
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Municipio)
                .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId, cancellationToken);

            if (usuario == null)
            {
                return ApiResponse<UsuarioResponse>.CreateError(
                    "Usuario no encontrado",
                    new List<string> { $"No existe un usuario con ID {usuarioId}" },
                    StatusCodes.Status404NotFound
                );
            }

            var response = new UsuarioResponse
            {
                UsuarioId = usuario.UsuarioId,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                MunicipioId = usuario.MunicipioId,
                MunicipioNombre = usuario.Municipio?.Nombre,
                RolId = usuario.RolId,
                NombreRol = usuario.Rol?.NombreRol ?? "Sin rol",
                Estado = usuario.Estado,
                FechaRegistro = usuario.FechaRegistro,
                FechaBaneo = usuario.FechaBaneo
            };

            return ApiResponse<UsuarioResponse>.CreateSuccess(response, "Usuario obtenido exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario con ID {UsuarioId}", usuarioId);
            return ApiResponse<UsuarioResponse>.CreateError(
                "Error al obtener el usuario",
                new List<string> { ex.Message },
                StatusCodes.Status500InternalServerError
            );
        }
    }

    public async Task<ApiResponse<UsuarioResponse>> CreateAsync(CreateUsuarioRequest request, int usuarioLogueadoId, CancellationToken cancellationToken = default)
    {
        try
        {
            var usuarioLogueado = await _context.Usuarios.FindAsync(new object[] { usuarioLogueadoId }, cancellationToken);
           
            if (usuarioLogueado == null)
            {
                return ApiResponse<UsuarioResponse>.CreateError(
                    "Usuario no autenticado",
                    new List<string> { "No se pudo identificar al usuario" },
                    StatusCodes.Status401Unauthorized
                );
            }

            if (usuarioLogueado.RolId != 3)
            {
                return ApiResponse<UsuarioResponse>.CreateError(
                    "Permisos insuficientes",
                    new List<string> { "Solo administradores pueden crear usuarios" },
                    StatusCodes.Status403Forbidden
                );
            }

            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == request.Email, cancellationToken);
            
            if (emailExiste)
            {
                return ApiResponse<UsuarioResponse>.CreateError(
                    "Email ya registrado",
                    new List<string> { "Este email ya tiene una cuenta" },
                    StatusCodes.Status409Conflict
                );
            }

            var municipioExiste = await _context.Municipios.AnyAsync(m => m.MunicipioId == request.MunicipioId, cancellationToken);
           
            if (!municipioExiste)
            {
                return ApiResponse<UsuarioResponse>.CreateError(
                    "Municipio inválido",
                    new List<string> { "El municipio no existe" },
                    StatusCodes.Status400BadRequest
                );
            }

            var rolExiste = await _context.Roles.AnyAsync(r => r.RolId == request.RolId, cancellationToken);
            
            if (!rolExiste)
            {
                return ApiResponse<UsuarioResponse>.CreateError(
                    "Rol inválido",
                    new List<string> { "El rol no existe" },
                    StatusCodes.Status400BadRequest
                );
            }

            var nuevoUsuario = new EcoMercaditoAPI.Models.Usuarios.Usuario
            {
                Nombre = request.Nombre,
                Email = request.Email,
                Telefono = request.Telefono,
                MunicipioId = request.MunicipioId,
                RolId = request.RolId,
                Estado = "Activo",
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(nuevoUsuario);

            await _context.SaveChangesAsync(cancellationToken);

            var credencial = new Credencial
            {
                UsuarioId = nuevoUsuario.UsuarioId,
                PasswordHash = HashPassword(request.Password),
                ProveedorAuth = "Local",
                FechaActualizacion = DateTime.Now
            };

            _context.Credenciales.Add(credencial);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new UsuarioResponse
            {
                UsuarioId = nuevoUsuario.UsuarioId,
                Nombre = nuevoUsuario.Nombre,
                Email = nuevoUsuario.Email,
                Telefono = nuevoUsuario.Telefono,
                MunicipioId = nuevoUsuario.MunicipioId,
                RolId = nuevoUsuario.RolId,
                NombreRol = request.RolId == 1 ? "Comprador" : request.RolId == 2 ? "Comercio" : "Administrador",
                Estado = nuevoUsuario.Estado,
                FechaRegistro = nuevoUsuario.FechaRegistro
            };

            return ApiResponse<UsuarioResponse>.CreateSuccess(response, "Usuario creado exitosamente", StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear usuario");
            return ApiResponse<UsuarioResponse>.CreateError(
                "Error al crear el usuario",
                new List<string> { ex.Message },
                StatusCodes.Status500InternalServerError
            );
        }
    }

    public async Task<ApiResponse<UsuarioResponse>> UpdateAsync(UpdateUsuarioRequest request, int usuarioLogueadoId, CancellationToken cancellationToken = default)
    {
        try
        {
            var usuarioLogueado = await _context.Usuarios.FindAsync(new object[] { usuarioLogueadoId }, cancellationToken);
            if (usuarioLogueado == null)
            {
                return ApiResponse<UsuarioResponse>.CreateError(
                    "Usuario no autenticado",
                    new List<string> { "No se pudo identificar al usuario" },
                    StatusCodes.Status401Unauthorized
                );
            }

            if (usuarioLogueado.UsuarioId != request.UsuarioId && usuarioLogueado.RolId != 3)
            {
                return ApiResponse<UsuarioResponse>.CreateError(
                    "Permisos insuficientes",
                    new List<string> { "Solo puedes editar tu propio usuario o ser administrador" },
                    StatusCodes.Status403Forbidden
                );
            }

            var usuario = await _context.Usuarios.FindAsync(new object[] { request.UsuarioId }, cancellationToken);
            if (usuario == null)
            {
                return ApiResponse<UsuarioResponse>.CreateError(
                    "Usuario no encontrado",
                    new List<string> { $"No existe usuario con ID {request.UsuarioId}" },
                    StatusCodes.Status404NotFound
                );
            }

            if (!string.IsNullOrEmpty(request.Email) && request.Email != usuario.Email)
            {
                var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == request.Email && u.UsuarioId != request.UsuarioId, cancellationToken);
                if (emailExiste)
                {
                    return ApiResponse<UsuarioResponse>.CreateError(
                        "Email ya registrado",
                        new List<string> { "Este email ya está en uso" },
                        StatusCodes.Status409Conflict
                    );
                }
                usuario.Email = request.Email;
            }

            if (!string.IsNullOrEmpty(request.Nombre))
                usuario.Nombre = request.Nombre;

            if (!string.IsNullOrEmpty(request.Telefono))
                usuario.Telefono = request.Telefono;

            if (request.MunicipioId.HasValue)
            {
                var municipioExiste = await _context.Municipios.AnyAsync(m => m.MunicipioId == request.MunicipioId, cancellationToken);
                if (!municipioExiste)
                {
                    return ApiResponse<UsuarioResponse>.CreateError(
                        "Municipio inválido",
                        new List<string> { "El municipio no existe" },
                        StatusCodes.Status400BadRequest
                    );
                }
                usuario.MunicipioId = request.MunicipioId.Value;
            }

            if (request.RolId.HasValue)
            {
                if (usuarioLogueado.RolId != 3)
                {
                    return ApiResponse<UsuarioResponse>.CreateError(
                        "Permisos insuficientes",
                        new List<string> { "Solo administradores pueden cambiar roles" },
                        StatusCodes.Status403Forbidden
                    );
                }

                var rolExiste = await _context.Roles.AnyAsync(r => r.RolId == request.RolId, cancellationToken);
                if (!rolExiste)
                {
                    return ApiResponse<UsuarioResponse>.CreateError(
                        "Rol inválido",
                        new List<string> { "El rol no existe" },
                        StatusCodes.Status400BadRequest
                    );
                }
                usuario.RolId = request.RolId.Value;
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                var credencial = await _context.Credenciales.FirstOrDefaultAsync(c => c.UsuarioId == request.UsuarioId, cancellationToken);
                if (credencial != null)
                {
                    credencial.PasswordHash = HashPassword(request.Password);
                    credencial.FechaActualizacion = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            var response = new UsuarioResponse
            {
                UsuarioId = usuario.UsuarioId,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                MunicipioId = usuario.MunicipioId,
                RolId = usuario.RolId,
                NombreRol = usuario.Rol?.NombreRol ?? "Sin rol",
                Estado = usuario.Estado,
                FechaRegistro = usuario.FechaRegistro
            };

            return ApiResponse<UsuarioResponse>.CreateSuccess(response, "Usuario actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar usuario {UsuarioId}", request.UsuarioId);
            return ApiResponse<UsuarioResponse>.CreateError(
                "Error al actualizar el usuario",
                new List<string> { ex.Message },
                StatusCodes.Status500InternalServerError
            );
        }
    }

    public async Task<ApiResponse<bool>> DesactivarAsync(int usuarioId, int usuarioLogueadoId, CancellationToken cancellationToken = default)
    {
        try
        {
            var usuarioLogueado = await _context.Usuarios.FindAsync(new object[] { usuarioLogueadoId }, cancellationToken);
            if (usuarioLogueado == null)
            {
                return ApiResponse<bool>.CreateError(
                    "Usuario no autenticado",
                    new List<string> { "No se pudo identificar al usuario" },
                    StatusCodes.Status401Unauthorized
                );
            }

            if (usuarioLogueado.RolId != 3)
            {
                return ApiResponse<bool>.CreateError(
                    "Permisos insuficientes",
                    new List<string> { "Solo administradores pueden desactivar usuarios" },
                    StatusCodes.Status403Forbidden
                );
            }

            var usuario = await _context.Usuarios.FindAsync(new object[] { usuarioId }, cancellationToken);
            if (usuario == null)
            {
                return ApiResponse<bool>.CreateError(
                    "Usuario no encontrado",
                    new List<string> { $"No existe usuario con ID {usuarioId}" },
                    StatusCodes.Status404NotFound
                );
            }

            if (usuario.UsuarioId == usuarioLogueadoId)
            {
                return ApiResponse<bool>.CreateError(
                    "Operación no permitida",
                    new List<string> { "No puedes desactivar tu propio usuario" },
                    StatusCodes.Status400BadRequest
                );
            }

            usuario.Estado = "Inactivo";
            usuario.FechaBaneo = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.CreateSuccess(true, "Usuario desactivado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al desactivar usuario {UsuarioId}", usuarioId);
            return ApiResponse<bool>.CreateError(
                "Error al desactivar el usuario",
                new List<string> { ex.Message },
                StatusCodes.Status500InternalServerError
            );
        }
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}
