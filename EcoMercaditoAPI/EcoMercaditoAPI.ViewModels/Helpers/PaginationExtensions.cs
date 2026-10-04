using Microsoft.EntityFrameworkCore;

namespace EcoMercaditoAPI.ViewModels.Helpers
{
    /// <summary>
    /// Métodos de extensión para facilitar la paginación de consultas.
    /// </summary>
    public static class PaginationExtensions
    {
        /// <summary>
        /// Aplica paginación a una consulta IQueryable y retorna los metadatos.
        /// </summary>
        public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await query.CountAsync(cancellationToken);
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return PagedResponse<T>.Create(data, pageNumber, pageSize, totalCount);
        }

        /// <summary>
        /// Aplica paginación a una consulta IQueryable usando PagedRequest.
        /// </summary>
        public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(
            this IQueryable<T> query,
            PagedRequest request,
            CancellationToken cancellationToken = default)
        {
            return await query.ToPagedResponseAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}
