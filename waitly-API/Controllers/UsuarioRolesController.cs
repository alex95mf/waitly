using iSit_API.Models.DTOs;
using iSit_API.Models.Entities;
using iSit_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iSit_API.Controllers
{
    [Route("api/v1/usuario-roles")]
    [ApiController]
    public class UsuarioRolesController : ApiControllerBase
    {
        private readonly IUsuarioRolService _usuarioRolService;

        public UsuarioRolesController(IUsuarioRolService usuarioRolService)
        {
            _usuarioRolService = usuarioRolService;
        }

        // GET: api/v1/usuario-roles/usuarios/{usuarioId}/roles
        [HttpGet("usuarios/{usuarioId}/roles")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<RolDTO>>>> GetRolesByUsuario(int usuarioId)
        {
            try
            {
                var roles = await _usuarioRolService.GetRolesByUsuarioIdAsync(usuarioId);
                return ApiOk(roles, $"Roles del usuario con ID {usuarioId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<RolDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<RolDTO>>($"Error al obtener los roles del usuario con ID {usuarioId}", ex);
            }
        }

        // GET: api/v1/usuario-roles/roles/{rolId}/usuarios
        [HttpGet("roles/{rolId}/usuarios")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UsuarioDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UsuarioDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UsuarioDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<UsuarioDTO>>>> GetUsuariosByRol(int rolId)
        {
            try
            {
                var usuarios = await _usuarioRolService.GetUsuariosByRolIdAsync(rolId);
                return ApiOk(usuarios, $"Usuarios con el rol ID {rolId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<UsuarioDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<UsuarioDTO>>($"Error al obtener los usuarios con el rol ID {rolId}", ex);
            }
        }

        // POST: api/v1/usuario-roles/usuarios/{usuarioId}/roles
        [HttpPost("usuarios/{usuarioId}/roles")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> AsignarRolesAUsuario(int usuarioId, [FromBody] List<int> rolIds)
        {
            try
            {
                await _usuarioRolService.AsignarRolesAUsuarioAsync(usuarioId, rolIds);
                return ApiOk<object>(null!, "Roles asignados al usuario exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al asignar roles al usuario", ex);
            }
        }

        // DELETE: api/v1/usuario-roles/usuarios/{usuarioId}/roles
        [HttpDelete("usuarios/{usuarioId}/roles")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> QuitarRolesDeUsuario(int usuarioId, [FromBody] List<int> rolIds)
        {
            try
            {
                await _usuarioRolService.QuitarRolesDeUsuarioAsync(usuarioId, rolIds);
                return ApiOk<object>(null!, "Roles eliminados del usuario exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar roles del usuario", ex);
            }
        }
    }
}