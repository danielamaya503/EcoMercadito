using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcoMercaditoAPI.ViewModels.Helpers
{

    /// <summary>
    /// Métodos de extensión para facilitar el retorno de respuestas en controladores.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Retorna una respuesta exitosa con datos.
        /// </summary>
        public static ActionResult<T> Success<T>(this ControllerBase controller, T data, string message = "Operación exitosa")
        {
            return controller.Ok(ApiResponse<T>.CreateSuccess(data, message));
        }

        /// <summary>
        /// Retorna una respuesta de recurso creado.
        /// </summary>
        public static ActionResult<T> Created<T>(this ControllerBase controller, T data, string routeName, object routeValues)
        {
            var response = ApiResponse<T>.CreateSuccess(data, "Recurso creado exitosamente", StatusCodes.Status201Created);
            return controller.CreatedAtRoute(routeName, routeValues, response);
        }

        /// <summary>
        /// Retorna una respuesta de no encontrado.
        /// </summary>
        public static ActionResult<T> NotFound<T>(this ControllerBase controller, string message = "Recurso no encontrado")
        {
            return controller.NotFound(ApiResponse<T>.CreateError(message, statusCode: StatusCodes.Status404NotFound));
        }

        /// <summary>
        /// Retorna una respuesta de error de validación.
        /// </summary>
        public static ActionResult<T> BadRequest<T>(this ControllerBase controller, string message = "Datos inválidos", List<string>? errors = null)
        {
            return controller.BadRequest(ApiResponse<T>.CreateError(message, errors, StatusCodes.Status400BadRequest));
        }

        /// <summary>
        /// Retorna una respuesta de error interno.
        /// </summary>
        public static ActionResult<T> InternalError<T>(this ControllerBase controller, string message = "Error interno del servidor")
        {
            return controller.StatusCode(500, ApiResponse<T>.CreateError(message, statusCode: StatusCodes.Status500InternalServerError));
        }

        /// <summary>
        /// Retorna una respuesta exitosa sin datos.
        /// </summary>
        public static ActionResult Success(this ControllerBase controller, string message = "Operación exitosa")
        {
            return controller.Ok(ApiResponse.CreateSuccess(message));
        }

        /// <summary>
        /// Retorna una respuesta de no contenido.
        /// </summary>
        public static ActionResult NoContent(this ControllerBase controller)
        {
            return controller.NoContent();
        }
    }
}
