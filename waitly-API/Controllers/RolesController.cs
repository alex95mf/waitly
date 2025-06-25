// Controllers/RolesController.cs
using waitly_API.Controllers;
using waitly_API.Models;
using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace waitly_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RolesController : ApiControllerBase
    {
        private readonly IRolService _rolService;

        public RolesController(IRolService rolService)
        {
            _rolService = rolService;
        }

        // GET: api/v1/Roles
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<RolDTO>>>> GetRoles()
        {
            try
            {
                var roles = await _rolService.GetAllRolesAsync();
                return ApiOk(roles, "Roles recuperados exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<RolDTO>>("Error al obtener los roles", ex);
            }
        }

        // GET: api/v1/Roles/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<RolDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<RolDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<RolDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<RolDTO>>> GetRol(int id)
        {
            try
            {
                var rol = await _rolService.GetRolByIdAsync(id);
                if (rol == null)
                {
                    return ApiNotFound<RolDTO>($"Rol con ID {id} no encontrado");
                }
                return ApiOk(rol, "Rol recuperado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<RolDTO>("Error al obtener el rol", ex);
            }
        }

        // PUT: api/v1/Roles/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<RolDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<RolDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<RolDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<RolDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<RolDTO>>> UpdateRol(int id, UpdateRolDTO updateRolDto)
        {
            try
            {
                bool result = await _rolService.UpdateRolAsync(id, updateRolDto);
                if (!result)
                {
                    return ApiNotFound<RolDTO>($"Rol con ID {id} no encontrado");
                }
                var updatedRol = await _rolService.GetRolByIdAsync(id);
                return ApiOk(updatedRol, "Rol actualizado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<RolDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<RolDTO>("Error al actualizar el rol", ex);
            }
        }

        // POST: api/v1/Roles
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<RolDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<RolDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<RolDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<RolDTO>>> CreateRol(CreateRolDTO createRolDto)
        {
            try
            {
                var rol = await _rolService.CreateRolAsync(createRolDto);
                return ApiCreated(
                    rol,
                    "Rol creado exitosamente",
                    nameof(GetRol),
                    new { id = rol.Id }
                );
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<RolDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<RolDTO>("Error al crear el rol", ex);
            }
        }

        // DELETE: api/v1/Roles/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteRol(int id)
        {
            try
            {
                bool result = await _rolService.DeleteRolAsync(id);
                if (!result)
                {
                    return ApiNotFound<object>($"Rol con ID {id} no encontrado");
                }
                return ApiOk<object>(null!, "Rol eliminado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar el rol", ex);
            }
        }

        // GET: api/v1/Roles/empresa/{empresaId}
        [HttpGet("empresa/{empresaId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<RolDTO>>>> GetRolesByEmpresa(int empresaId)
        {
            try
            {
                var roles = await _rolService.GetRolesByEmpresaIdAsync(empresaId);
                return ApiOk(roles, $"Roles de la empresa con ID {empresaId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<RolDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<RolDTO>>($"Error al obtener los roles de la empresa con ID {empresaId}", ex);
            }
        }
    }
}