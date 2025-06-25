using waitly_API.Controllers;
using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using waitly_API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace waitly_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuariosController : ApiControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/v1/Usuarios
        [HttpGet("admin-only")]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UsuarioDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UsuarioDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<UsuarioDTO>>>> GetUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllUsuariosAsync();
                return ApiOk(usuarios, "Usuarios recuperados exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<UsuarioDTO>>("Error al obtener los usuarios", ex);
            }
        }

        // GET: api/v1/Usuarios/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UsuarioDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> GetUsuario(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByIdAsync(id);

                if (usuario == null)
                {
                    return ApiNotFound<UsuarioDTO>($"Usuario con ID {id} no encontrado");
                }

                return ApiOk(usuario, "Usuario recuperado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<UsuarioDTO>("Error al obtener el usuario", ex);
            }
        }

        // PUT: api/v1/Usuarios/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UsuarioDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> UpdateUsuario(int id, UpdateUsuarioDTO updateUsuarioDto)
        {
            try
            {
                bool result = await _usuarioService.UpdateUsuarioAsync(id, updateUsuarioDto);

                if (!result)
                {
                    return ApiNotFound<UsuarioDTO>($"Usuario con ID {id} no encontrado");
                }

                var updatedUsuario = await _usuarioService.GetUsuarioByIdAsync(id);
                return ApiOk(updatedUsuario, "Usuario actualizado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<UsuarioDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<UsuarioDTO>("Error al actualizar el usuario", ex);
            }
        }

        // POST: api/v1/Usuarios
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<UsuarioDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<UsuarioDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> CreateUsuario(CreateUsuarioDTO createUsuarioDto)
        {
            try
            {
                var authResponse = await _usuarioService.CreateUsuarioAsync(createUsuarioDto);
                return ApiCreated(
                    authResponse,
                    "Usuario creado exitosamente",
                    nameof(GetUsuario),
                    new { id = authResponse.Usuario.Id }
                );
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<AuthResponseDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<AuthResponseDTO>("Error al crear el usuario", ex);
            }
        }

        // DELETE: api/v1/Usuarios/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteUsuario(int id)
        {
            try
            {
                bool result = await _usuarioService.DeleteUsuarioAsync(id);

                if (!result)
                {
                    return ApiNotFound<object>($"Usuario con ID {id} no encontrado");
                }

                return ApiOk<object>(null!, "Usuario eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar el usuario", ex);
            }
        }

        // POST: api/v1/Usuarios/login
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> Login(LoginUsuarioDTO loginDto)
        {
            try
            {
                var authResponse = await _usuarioService.AuthenticateAsync(loginDto);

                if (authResponse == null)
                {
                    return ApiUnauthorized<AuthResponseDTO>("Credenciales inválidas");
                }

                return ApiOk(authResponse, "Inicio de sesión exitoso");
            }
            catch (Exception ex)
            {
                return ApiServerError<AuthResponseDTO>("Error durante el inicio de sesión", ex);
            }
        }

        // POST: api/v1/Usuarios/5/change-password
        [HttpPost("{id}/change-password")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword(int id, ChangePasswordDTO changePasswordDto)
        {
            try
            {
                bool result = await _usuarioService.ChangePasswordAsync(id, changePasswordDto);

                if (!result)
                {
                    return ApiNotFound<object>($"Usuario con ID {id} no encontrado");
                }

                return ApiOk<object>(null!, "Contraseña actualizada exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al cambiar la contraseña", ex);
            }
        }
    }

}