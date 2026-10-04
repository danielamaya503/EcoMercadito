using EcoMercaditoAPI.Concretes.Context;
using EcoMercaditoAPI.Interfaces.Usuaio;
using EcoMercaditoAPI.Models.Usuarios;
using EcoMercaditoAPI.ViewModels.Helpers;
using EcoMercaditoAPI.ViewModels.Usuario.Request;
using EcoMercaditoAPI.ViewModels.Usuario.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace EcoMercaditoAPI.Concretes.Usuario;

public class AuthentificacionService : IAuthentificacion
{
    private readonly AppDbContext context;
    private readonly ILogger<AuthentificacionService> logger;

    public AuthentificacionService(AppDbContext context, ILogger<AuthentificacionService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        try 
        {
            var usuarioLogin = await context.Usuarios
                .Include(u => u.Rol)          
                .Include(u => u.Municipio)    
                .Include(u => u.Credencial)
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (usuarioLogin == null)
            {
                return ApiResponse<AuthResponse>.CreateError(
                   "Credenciales inválidas",
                   new List<string> { "El correo electrónico o la contraseña son incorrectos" },
                   StatusCodes.Status401Unauthorized
               );
            }

            var credenciales = usuarioLogin.Credencial;

            if (credenciales is null)
            {
                return ApiResponse<AuthResponse>.CreateError(
                   "Credenciales inválidas",
                   new List<string> { "El usuario no tiene credenciales registradas" },
                   StatusCodes.Status401Unauthorized
               );
            }

            var passwordHash = HashPassword(request.Password);

            if (credenciales.PasswordHash != passwordHash)
            {
                return ApiResponse<AuthResponse>.CreateError(
                   "Credenciales inválidas",
                   new List<string> { "El correo electrónico o la contraseña son incorrectos" },
                   StatusCodes.Status401Unauthorized
               );
            }

            if(usuarioLogin.Estado == "Inactivo")
            {
                return ApiResponse<AuthResponse>.CreateError(
                   "Cuenta desactivada",
                   new List<string> { "La cuenta asociada al correo electrónico está desactivada" },
                   StatusCodes.Status401Unauthorized
               );
            }

            var token = GenerateToken(usuarioLogin.UsuarioId);

            var expiracion = DateTime.UtcNow.AddHours(24);

            var authResponse = new AuthResponse
            {
                UsuarioId = usuarioLogin.UsuarioId,
                Email = usuarioLogin.Email,
                Nombre = usuarioLogin.Nombre,
                Telefono = usuarioLogin.Telefono,
                NombreRol = usuarioLogin.Rol!.NombreRol,
                RolId = usuarioLogin.RolId,
                MunicipioId = usuarioLogin.MunicipioId,
                MunicipioNombre = usuarioLogin.Municipio?.Nombre,
                Estado = usuarioLogin.Estado,
                Token = token,
                Expiracion = expiracion
            };

            return ApiResponse<AuthResponse>.CreateSuccess(
               authResponse,
               "Login exitoso",
               StatusCodes.Status200OK
           );

        }
        catch (Exception ex) 
        {
            logger.LogError(ex, "Error occurred while logging in");
            return ApiResponse<AuthResponse>.CreateError(
                            "Error interno del servidor",
                            new List<string> { "Ocurrió un error inesperado. Intente más tarde." },
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

    private string GenerateToken(int usuarioId)
    {
        // Token simple: GUID + timestamp + usuarioId
        var timestamp = DateTime.UtcNow.Ticks;
        var random = Guid.NewGuid().ToString("N")[..16];
        return $"usr_{usuarioId}_{random}_{timestamp}";
    }
}
