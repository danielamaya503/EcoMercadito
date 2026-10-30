namespace EcoMercaditoAPI.ViewModels.EcoMercadito.Response;

public class NegocioResponse
{
    public int NegocioId { get; set; }

    public int UsuarioId { get; set; }

    public string NombreUsuario { get; set; } = string.Empty;

    public string NombreComercial { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public int MunicipioId { get; set; }

    public string MunicipioNombre { get; set; } = string.Empty;

    public string DepartamentoNombre { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }
}