using iSit_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iSit_API.Services.Interfaces;
using iSit_API.Models.Entities;

namespace iSit_API.Controllers
{
    [Route("api/v1/usuarios-empresas")]
    [ApiController]
    public class UsuarioEmpresasController : ApiControllerBase
    {
        private readonly IUsuarioEmpresaService _usuarioEmpresaService;

        public UsuarioEmpresasController(IUsuarioEmpresaService usuarioEmpresaService)
        {
            _usuarioEmpresaService = usuarioEmpresaService;
        }

        // GET: api/v1/usuarios-empresas/usuarios/{usuarioId}/empresas
        [HttpGet("usuarios/{usuarioId}/empresas")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpresaDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpresaDTO>>>> GetEmpresasByUsuario(int usuarioId)
        {
            try
            {
                var empresas = await _usuarioEmpresaService.GetEmpresasByUsuarioIdAsync(usuarioId);
                return ApiOk(empresas, "Empresas del usuario recuperadas exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<EmpresaDTO>>("Error al obtener las empresas del usuario", ex);
            }
        }

        // POST: api/v1/usuarios-empresas/usuarios/{usuarioId}/empresas
        [HttpPost("usuarios/{usuarioId}/empresas")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<object>>> AsignarEmpresasAUsuario(int usuarioId, [FromBody] List<int> empresaIds)
        {
            try
            {
                await _usuarioEmpresaService.AsignarEmpresasAUsuarioAsync(usuarioId, empresaIds);
                return ApiOk<object>(null!, "Empresas asignadas al usuario exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al asignar empresas al usuario", ex);
            }
        }

        // DELETE: api/v1/usuarios-empresas/usuarios/{usuarioId}/empresas
        [HttpDelete("usuarios/{usuarioId}/empresas")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<object>>> QuitarEmpresasDeUsuario(int usuarioId, [FromBody] List<int> empresaIds)
        {
            try
            {
                await _usuarioEmpresaService.QuitarEmpresasDeUsuarioAsync(usuarioId, empresaIds);
                return ApiOk<object>(null!, "Empresas eliminadas del usuario exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar empresas del usuario", ex);
            }
        }

        // GET: api/v1/usuarios-empresas/empresas/{empresaId}/usuarios
        [HttpGet("empresas/{empresaId}/usuarios")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UsuarioDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<UsuarioDTO>>>> GetUsuariosByEmpresa(int empresaId)
        {
            try
            {
                var usuarios = await _usuarioEmpresaService.GetUsuariosByEmpresaIdAsync(empresaId);
                return ApiOk(usuarios, "Usuarios de la empresa recuperados exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<UsuarioDTO>>("Error al obtener los usuarios de la empresa", ex);
            }
        }
    }
}
