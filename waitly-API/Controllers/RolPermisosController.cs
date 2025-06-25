using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace waitly_API.Controllers
{
    [Route("api/v1/roles-permisos")]
    [ApiController]
    public class RolPermisosController : ApiControllerBase
    {
        private readonly IRolPermisoService _rolPermisoService;

        public RolPermisosController(IRolPermisoService rolPermisoService)
        {
            _rolPermisoService = rolPermisoService;
        }

        // GET: api/v1/roles-permisos/roles/{rolId}/permisos
        [HttpGet("roles/{rolId}/permisos")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PermisoDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PermisoDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PermisoDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PermisoDTO>>>> GetPermisosByRol(int rolId)
        {
            try
            {
                var permisos = await _rolPermisoService.GetPermisosByRolIdAsync(rolId);
                return ApiOk(permisos, $"Permisos del rol con ID {rolId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<PermisoDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<PermisoDTO>>($"Error al obtener los permisos del rol con ID {rolId}", ex);
            }
        }

        // GET: api/v1/roles-permisos/permisos/{permisoId}/roles
        [HttpGet("permisos/{permisoId}/roles")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<RolDTO>>>> GetRolesByPermiso(int permisoId)
        {
            try
            {
                var roles = await _rolPermisoService.GetRolesByPermisoIdAsync(permisoId);
                return ApiOk(roles, $"Roles con el permiso ID {permisoId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<RolDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<RolDTO>>($"Error al obtener los roles con el permiso ID {permisoId}", ex);
            }
        }

        // POST: api/v1/roles-permisos/roles/{rolId}/permisos
        [HttpPost("roles/{rolId}/permisos")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> AsignarPermisosARol(int rolId, [FromBody] List<int> permisoIds)
        {
            try
            {
                await _rolPermisoService.AsignarPermisosARolAsync(rolId, permisoIds);
                return ApiOk<object>(null!, "Permisos asignados al rol exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al asignar permisos al rol", ex);
            }
        }

        // DELETE: api/v1/roles-permisos/roles/{rolId}/permisos
        [HttpDelete("roles/{rolId}/permisos")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> QuitarPermisosDeRol(int rolId, [FromBody] List<int> permisoIds)
        {
            try
            {
                await _rolPermisoService.QuitarPermisosDeRolAsync(rolId, permisoIds);
                return ApiOk<object>(null!, "Permisos eliminados del rol exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar permisos del rol", ex);
            }
        }
    }
}