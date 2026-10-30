using System.ComponentModel.DataAnnotations;

namespace EcoMercaditoAPI.ViewModels.EcoMercadito;

public class CrearNegocioRequest
{

    [Required(ErrorMessage = "El MunicipioId es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El MunicipioId debe ser mayor a cero.")]
    public int MunicipioId { get; set; }

    [Required(ErrorMessage = "El nombre comercial es obligatorio.")]
    [StringLength(200, MinimumLength = 3,
        ErrorMessage = "El nombre comercial debe tener entre 3 y 200 caracteres.")]
    public string NombreComercial { get; set; } = string.Empty;

    [StringLength(1000,
        ErrorMessage = "La descripción no puede exceder 1000 caracteres.")]
    public string? Descripcion { get; set; }
}