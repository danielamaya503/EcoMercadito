using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.ViewModels.Usuario.Response;

public class UsuarioResponse
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public int MunicipioId { get; set; }
    public string? MunicipioNombre { get; set; }
    public int RolId { get; set; }
    public string NombreRol { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaBaneo { get; set; }
}