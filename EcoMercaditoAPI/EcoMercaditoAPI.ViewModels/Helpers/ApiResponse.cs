using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace EcoMercaditoAPI.ViewModels.Helpers
{
    /// <summary>
    /// Representa una respuesta estándar de la API.
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje descriptivo del resultado.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Datos de la respuesta (solo si Success es true).
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Lista de errores (solo si Success es false).
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// Código de estado HTTP asociado.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Crea una respuesta exitosa.
        /// </summary>
        public static ApiResponse<T> CreateSuccess(T data, string message = "Operación exitosa", int statusCode = StatusCodes.Status200OK)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Crea una respuesta de error.
        /// </summary>
        public static ApiResponse<T> CreateError(string message, List<string>? errors = null, int statusCode = StatusCodes.Status400BadRequest)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                StatusCode = statusCode
            };
        }
    }

    /// <summary>
    /// Clase no genérica para respuestas sin datos.
    /// </summary>
    public class ApiResponse : ApiResponse<object>
    {
        public static ApiResponse CreateSuccess(string message = "Operación exitosa", int statusCode = StatusCodes.Status200OK)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static ApiResponse CreateError(string message, List<string>? errors = null, int statusCode = StatusCodes.Status400BadRequest)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                StatusCode = statusCode
            };
        }
    }
}
