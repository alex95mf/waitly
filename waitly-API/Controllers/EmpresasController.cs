using waitly_API.Controllers;
using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace waitly_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmpresasController : ApiControllerBase
    {
        private readonly IEmpresaService _empresaService;

        public EmpresasController(IEmpresaService empresaService)
        {
            _empresaService = empresaService;
        }

        // GET: api/v1/Empresas
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpresaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpresaDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpresaDTO>>>> GetEmpresas()
        {
            try
            {
                var empresas = await _empresaService.GetAllEmpresasAsync();
                return ApiOk(empresas, "Empresas recuperadas exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<EmpresaDTO>>("Error al obtener las empresas", ex);
            }
        }

        // GET: api/v1/Empresas/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<EmpresaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<EmpresaDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<EmpresaDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<EmpresaDTO>>> GetEmpresa(int id)
        {
            try
            {
                var empresa = await _empresaService.GetEmpresaByIdAsync(id);
                if (empresa == null)
                {
                    return ApiNotFound<EmpresaDTO>($"Empresa con ID {id} no encontrada");
                }
                return ApiOk(empresa, "Empresa recuperada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<EmpresaDTO>("Error al obtener la empresa", ex);
            }
        }

        // PUT: api/v1/Empresas/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<EmpresaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<EmpresaDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<EmpresaDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<EmpresaDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<EmpresaDTO>>> PutEmpresa(int id, UpdateEmpresaDTO updateEmpresaDto)
        {
            try
            {
                bool result = await _empresaService.UpdateEmpresaAsync(id, updateEmpresaDto);
                if (!result)
                {
                    return ApiNotFound<EmpresaDTO>($"Empresa con ID {id} no encontrada");
                }
                var updatedEmpresa = await _empresaService.GetEmpresaByIdAsync(id);
                return ApiOk(updatedEmpresa, "Empresa actualizada exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<EmpresaDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<EmpresaDTO>("Error al actualizar la empresa", ex);
            }
        }

        // POST: api/v1/Empresas
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<EmpresaDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<EmpresaDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<EmpresaDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<EmpresaDTO>>> PostEmpresa(CreateEmpresaDTO createEmpresaDto)
        {
            try
            {
                var empresa = await _empresaService.CreateEmpresaAsync(createEmpresaDto);
                return ApiCreated(
                    empresa,
                    "Empresa creada exitosamente",
                    nameof(GetEmpresa),
                    new { id = empresa.Id }
                );
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<EmpresaDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<EmpresaDTO>("Error al crear la empresa", ex);
            }
        }

        // DELETE: api/v1/Empresas/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteEmpresa(int id)
        {
            try
            {
                bool result = await _empresaService.DeleteEmpresaAsync(id);
                if (!result)
                {
                    return ApiNotFound<object>($"Empresa con ID {id} no encontrada");
                }
                return ApiOk<object>(null!, "Empresa eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar la empresa", ex);
            }
        }
    }
}