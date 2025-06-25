using System.Net;

namespace waitly_API.Models.Entities
{
    /// <summary>
    /// Clase estándar para todas las respuestas de la API
    /// </summary>
    /// <typeparam name="T">Tipo de datos que se devolverá en la propiedad Data</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje descriptivo sobre el resultado de la operación
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Datos devueltos por la operación
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Código de estado HTTP
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Errores adicionales (útil para validaciones)
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Información de metadatos (paginación, totales, etc.)
        /// </summary>
        public object Metadata { get; set; }

        /// <summary>
        /// Identificador de la solicitud (útil para seguimiento y logging)
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Marca de tiempo de la respuesta
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        public ApiResponse()
        {
            Success = false;
            Message = string.Empty;
            StatusCode = (int)HttpStatusCode.OK;
        }

        /// <summary>
        /// Crea una respuesta exitosa
        /// </summary>
        /// <param name="data">Datos a devolver</param>
        /// <param name="message">Mensaje opcional</param>
        /// <returns>Respuesta de API exitosa</returns>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operación realizada con éxito")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        /// <summary>
        /// Crea una respuesta de error
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="statusCode">Código de estado HTTP</param>
        /// <param name="errors">Lista de errores opcional</param>
        /// <returns>Respuesta de API con error</returns>
        public static ApiResponse<T> ErrorResponse(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, List<string> errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = (int)statusCode,
                Errors = errors
            };
        }

        /// <summary>
        /// Crea una respuesta con datos paginados
        /// </summary>
        /// <param name="data">Datos paginados</param>
        /// <param name="metadata">Información de paginación</param>
        /// <param name="message">Mensaje opcional</param>
        /// <returns>Respuesta de API con datos paginados</returns>
        public static ApiResponse<T> PaginatedResponse(T data, object metadata, string message = "Datos recuperados con éxito")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = (int)HttpStatusCode.OK,
                Metadata = metadata
            };
        }
    }
}
