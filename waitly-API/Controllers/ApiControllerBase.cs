using iSit_API.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace iSit_API.Controllers
{
    /// <summary>
    /// Controlador base que proporciona métodos comunes para respuestas de API estandarizadas
    /// </summary>
    public abstract class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// Crea una respuesta exitosa con datos
        /// </summary>
        protected ActionResult<ApiResponse<T>> ApiOk<T>(T data, string message = "Operación realizada con éxito")
        {
            var response = ApiResponse<T>.SuccessResponse(data, message);
            return Ok(response);
        }

        /// <summary>
        /// Crea una respuesta exitosa 201 Created con datos
        /// </summary>
        protected ActionResult<ApiResponse<T>> ApiCreated<T>(T data, string message = "Recurso creado con éxito", string? actionName = null, object? routeValues = null)
        {
            var response = ApiResponse<T>.SuccessResponse(data, message);
            response.StatusCode = (int)HttpStatusCode.Created;

            if (actionName != null)
            {
                return CreatedAtAction(actionName, routeValues, response);
            }

            return StatusCode(StatusCodes.Status201Created, response);
        }

        /// <summary>
        /// Crea una respuesta con estado NotFound 404
        /// </summary>
        protected ActionResult<ApiResponse<T>> ApiNotFound<T>(string message = "Recurso no encontrado")
        {
            var response = ApiResponse<T>.ErrorResponse(message, HttpStatusCode.NotFound);
            return NotFound(response);
        }

        /// <summary>
        /// Crea una respuesta con estado BadRequest 400
        /// </summary>
        protected ActionResult<ApiResponse<T>> ApiBadRequest<T>(string message = "Solicitud inválida", List<string>? errors = null)
        {
            var response = ApiResponse<T>.ErrorResponse(message, HttpStatusCode.BadRequest, errors);
            return BadRequest(response);
        }

        /// <summary>
        /// Crea una respuesta con estado Unauthorized 401
        /// </summary>
        protected ActionResult<ApiResponse<T>> ApiUnauthorized<T>(string message = "No autorizado")
        {
            var response = ApiResponse<T>.ErrorResponse(message, HttpStatusCode.Unauthorized);
            return Unauthorized(response);
        }

        /// <summary>
        /// Crea una respuesta con estado Forbidden 403
        /// </summary>
        protected ActionResult<ApiResponse<T>> ApiForbidden<T>(string message = "Acceso prohibido")
        {
            var response = ApiResponse<T>.ErrorResponse(message, HttpStatusCode.Forbidden);
            return StatusCode(StatusCodes.Status403Forbidden, response);
        }

        /// <summary>
        /// Crea una respuesta con estado InternalServerError 500
        /// </summary>
        protected ActionResult<ApiResponse<T>> ApiServerError<T>(string message = "Error interno del servidor", Exception? ex = null)
        {
            List<string>? errors = null;

            if (ex != null)
            {
                errors = new List<string> { ex.Message };
                // Opcionalmente, agregar más detalles en entorno de desarrollo
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    errors.Add(ex.StackTrace ?? "No se ha proporcionado stack trace");
                }
            }

            var response = ApiResponse<T>.ErrorResponse(message, HttpStatusCode.InternalServerError, errors);
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }

        /// <summary>
        /// Crea una respuesta paginada
        /// </summary>
        protected ActionResult<ApiResponse<T>> ApiPaginated<T>(
            T data,
            int totalCount,
            int pageNumber,
            int pageSize,
            string message = "Datos recuperados con éxito")
        {
            var metadata = new
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                HasNext = pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize),
                HasPrevious = pageNumber > 1
            };

            var response = ApiResponse<T>.PaginatedResponse(data, metadata, message);
            return Ok(response);
        }
    }
}
