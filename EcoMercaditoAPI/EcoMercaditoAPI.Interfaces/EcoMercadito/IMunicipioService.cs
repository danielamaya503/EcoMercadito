using EcoMercaditoAPI.Helpers;
using EcoMercaditoAPI.Models.EcoMercadito;
using EcoMercaditoAPI.ViewModels.EcoMercadito.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.Interfaces.EcoMercadito
{
    public interface IMunicipioService
    {
        /// <summary>
        /// Obtiene todos los municipios paginados.
        /// </summary>
        /// <param name="pageNumber">Número de página (1-based).</param>
        /// <param name="pageSize">Cantidad de elementos por página (1-100).</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        Task<PagedResponse<MunicipioResponse>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene un municipio por su ID.
        /// </summary>
        Task<MunicipioDetalleResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene municipios por departamento.
        /// </summary>
        Task<PagedResponse<MunicipioResponse>> GetByDepartamentoIdPagedAsync(int departamentoId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        /// <summary>
        /// Busca municipios por nombre.
        /// </summary>
        Task<PagedResponse<MunicipioResponse>> SearchByNamePagedAsync(string nombre, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}
