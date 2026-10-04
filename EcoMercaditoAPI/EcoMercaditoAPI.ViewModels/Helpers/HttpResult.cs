using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcoMercaditoAPI.ViewModels.Helpers
{
    public class HttpResult<T>
    {
        /// <summary>
        /// Datos de la respuesta.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Código de estado HTTP.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Mensaje opcional.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Errores de validación.
        /// </summary>
        public Dictionary<string, List<string>>? ValidationErrors { get; set; }

        /// <summary>
        /// Crea un resultado exitoso.
        /// </summary>
        public static HttpResult<T> Ok(T data, string? message = null)
        {
            return new HttpResult<T>
            {
                Data = data,
                StatusCode = StatusCodes.Status200OK,
                Message = message
            };
        }

        /// <summary>
        /// Crea un resultado de recurso creado.
        /// </summary>
        public static HttpResult<T> Created(T data, string? message = null)
        {
            return new HttpResult<T>
            {
                Data = data,
                StatusCode = StatusCodes.Status201Created,
                Message = message ?? "Recurso creado exitosamente"
            };
        }

        /// <summary>
        /// Crea un resultado de no encontrado.
        /// </summary>
        public static HttpResult<T> NotFound(string message = "Recurso no encontrado")
        {
            return new HttpResult<T>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = message
            };
        }

        /// <summary>
        /// Crea un resultado de error de validación.
        /// </summary>
        public static HttpResult<T> BadRequest(string message = "Datos inválidos", Dictionary<string, List<string>>? errors = null)
        {
            return new HttpResult<T>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = message,
                ValidationErrors = errors
            };
        }

        /// <summary>
        /// Crea un resultado de error interno.
        /// </summary>
        public static HttpResult<T> InternalError(string message = "Error interno del servidor")
        {
            return new HttpResult<T>
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = message
            };
        }
    }

    /// <summary>
    /// Clase no genérica para resultados sin datos.
    /// </summary>
    public class HttpResult : HttpResult<object>
    {
        public static HttpResult Ok(string? message = null)
        {
            return new HttpResult
            {
                StatusCode = StatusCodes.Status200OK,
                Message = message ?? "Operación exitosa"
            };
        }

        public static HttpResult NoContent()
        {
            return new HttpResult
            {
                StatusCode = StatusCodes.Status204NoContent
            };
        }

        public static HttpResult NotFound(string message = "Recurso no encontrado")
        {
            return new HttpResult
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = message
            };
        }

        public static HttpResult BadRequest(string message = "Datos inválidos", Dictionary<string, List<string>>? errors = null)
        {
            return new HttpResult
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = message,
                ValidationErrors = errors
            };
        }

        public static HttpResult InternalError(string message = "Error interno del servidor")
        {
            return new HttpResult
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = message
            };
        }

    }
}
