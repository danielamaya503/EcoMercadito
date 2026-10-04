using EcoMercaditoAPI.ViewModels.Helpers;
using EcoMercaditoAPI.ViewModels.Usuario.Request;
using EcoMercaditoAPI.ViewModels.Usuario.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Interfaces.Usuaio;

public interface IAuthentificacion
{
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

}
