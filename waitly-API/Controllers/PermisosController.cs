using iSit_API.Models.DTOs;
using iSit_API.Models.Entities;
using iSit_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iSit_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PermisosController : ApiControllerBase
    {
        private readonly IPermisoService _permisoService;

        public PermisosController(IPermisoService permisoService)
        {
            _permisoService = permisoService;
        }

        // GET: api/v1/Permisos
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PermisoDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PermisoDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PermisoDTO>>>> GetPermisos()
        {
            try
            {
                var permisos = await _permisoService.GetAllPermisosAsync();
                return ApiOk(permisos, "Permisos recuperados exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<PermisoDTO>>("Error al obtener los permisos", ex);
            }
        }

        // GET: api/v1/Permisos/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PermisoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PermisoDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<PermisoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PermisoDTO>>> GetPermiso(int id)
        {
            try
            {
                var permiso = await _permisoService.GetPermisoByIdAsync(id);
                if (permiso == null)
                {
                    return ApiNotFound<PermisoDTO>($"Permiso con ID {id} no encontrado");
                }
                return ApiOk(permiso, "Permiso recuperado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<PermisoDTO>("Error al obtener el permiso", ex);
            }
        }

        // PUT: api/v1/Permisos/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PermisoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PermisoDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PermisoDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<PermisoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PermisoDTO>>> UpdatePermiso(int id, UpdatePermisoDTO updatePermisoDto)
        {
            try
            {
                bool result = await _permisoService.UpdatePermisoAsync(id, updatePermisoDto);
                if (!result)
                {
                    return ApiNotFound<PermisoDTO>($"Permiso con ID {id} no encontrado");
                }
                var updatedPermiso = await _permisoService.GetPermisoByIdAsync(id);
                return ApiOk(updatedPermiso, "Permiso actualizado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<PermisoDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<PermisoDTO>("Error al actualizar el permiso", ex);
            }
        }

        // POST: api/v1/Permisos
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PermisoDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<PermisoDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PermisoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PermisoDTO>>> CreatePermiso(CreatePermisoDTO createPermisoDto)
        {
            try
            {
                var permiso = await _permisoService.CreatePermisoAsync(createPermisoDto);
                return ApiCreated(
                    permiso,
                    "Permiso creado exitosamente",
                    nameof(GetPermiso),
                    new { id = permiso.Id }
                );
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<PermisoDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<PermisoDTO>("Error al crear el permiso", ex);
            }
        }

        // DELETE: api/v1/Permisos/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeletePermiso(int id)
        {
            try
            {
                bool result = await _permisoService.DeletePermisoAsync(id);
                if (!result)
                {
                    return ApiNotFound<object>($"Permiso con ID {id} no encontrado");
                }
                return ApiOk<object>(null!, "Permiso eliminado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar el permiso", ex);
            }
        }

        // GET: api/v1/Permisos/usuario/{usuarioId}
        [HttpGet("usuario/{usuarioId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PermisoDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PermisoDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PermisoDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PermisoDTO>>>> GetPermisosByUsuario(int usuarioId)
        {
            try
            {
                var permisos = await _permisoService.GetPermisosByUsuarioIdAsync(usuarioId);
                return ApiOk(permisos, $"Permisos del usuario con ID {usuarioId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<PermisoDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<PermisoDTO>>($"Error al obtener los permisos del usuario con ID {usuarioId}", ex);
            }
        }
    }
}