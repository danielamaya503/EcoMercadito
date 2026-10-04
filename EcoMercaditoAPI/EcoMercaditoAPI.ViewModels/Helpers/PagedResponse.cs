namespace EcoMercaditoAPI.ViewModels.Helpers
{
    /// <summary>
    /// Representa una respuesta paginada estándar.
    /// </summary>
    public class PagedResponse<T>
    {
        /// <summary>
        /// Lista de elementos de la página actual.
        /// </summary>
        public List<T> Data { get; set; } = new();

        /// <summary>
        /// Número de página actual (1-based).
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Cantidad de elementos por página.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total de páginas disponibles.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Total de elementos en la colección completa.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Indica si existe una página anterior.
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// Indica si existe una página siguiente.
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// Crea una respuesta paginada.
        /// </summary>
        public static PagedResponse<T> Create(List<T> data, int currentPage, int pageSize, int totalCount)
        {
            return new PagedResponse<T>
            {
                Data = data,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                TotalCount = totalCount
            };
        }
    }

    /// <summary>
    /// Parámetros de paginación para requests.
    /// </summary>
    public class PagedRequest
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;

        /// <summary>
        /// Número de página (mínimo 1).
        /// </summary>
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        /// <summary>
        /// Tamaño de página (1-100, default 10).
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 10 : (value > 100 ? 100 : value);
        }
    }
}
