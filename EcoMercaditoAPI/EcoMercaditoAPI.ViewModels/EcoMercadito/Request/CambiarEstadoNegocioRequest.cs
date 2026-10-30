using System.ComponentModel.DataAnnotations;

namespace EcoMercaditoAPI.ViewModels.EcoMercadito;

public class CambiarEstadoNegocioRequest
{
    [Required(ErrorMessage = "El estado es obligatorio.")]
    [RegularExpression(
        "^(Activo|Inactivo)$",
        ErrorMessage = "El estado debe ser Activo o Inactivo.")]
    public string Estado { get; set; } = string.Empty;
}