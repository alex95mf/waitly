using iSit_API.Models.DTOs;
using iSit_API.Models.Entities;
using iSit_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iSit_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogosController : ApiControllerBase
    {
        private readonly ICatalogoService _catalogoService;

        public CatalogosController(ICatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        // GET: api/v1/Catalogos
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CatalogoDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CatalogoDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CatalogoDTO>>>> GetCatalogos()
        {
            try
            {
                var catalogos = await _catalogoService.GetAllCatalogosAsync();
                return ApiOk(catalogos, "Catálogos recuperados exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<CatalogoDTO>>("Error al obtener los catálogos", ex);
            }
        }

        // GET: api/v1/Catalogos/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CatalogoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CatalogoDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<CatalogoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CatalogoDTO>>> GetCatalogo(int id)
        {
            try
            {
                var catalogo = await _catalogoService.GetCatalogoByIdAsync(id);
                if (catalogo == null)
                {
                    return ApiNotFound<CatalogoDTO>($"Catálogo con ID {id} no encontrado");
                }
                return ApiOk(catalogo, "Catálogo recuperado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<CatalogoDTO>("Error al obtener el catálogo", ex);
            }
        }

        // GET: api/v1/Catalogos/padre/{padreId}
        [HttpGet("padre/{padreId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CatalogoDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CatalogoDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CatalogoDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CatalogoDTO>>>> GetCatalogosByPadre(int padreId)
        {
            try
            {
                var catalogos = await _catalogoService.GetCatalogosByPadreIdAsync(padreId);
                return ApiOk(catalogos, $"Catálogos con padre ID {padreId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<CatalogoDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<CatalogoDTO>>($"Error al obtener los catálogos con padre ID {padreId}", ex);
            }
        }

        // GET: api/v1/Catalogos/raiz
        [HttpGet("raiz")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CatalogoDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CatalogoDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CatalogoDTO>>>> GetCatalogosRaiz()
        {
            try
            {
                // Pasamos null para obtener los catálogos raíz (sin padre)
                var catalogos = await _catalogoService.GetCatalogosByPadreIdAsync(null);
                return ApiOk(catalogos, "Catálogos raíz recuperados exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<CatalogoDTO>>("Error al obtener los catálogos raíz", ex);
            }
        }

        // PUT: api/v1/Catalogos/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CatalogoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CatalogoDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<CatalogoDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<CatalogoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CatalogoDTO>>> UpdateCatalogo(int id, UpdateCatalogoDTO updateCatalogoDto)
        {
            try
            {
                bool result = await _catalogoService.UpdateCatalogoAsync(id, updateCatalogoDto);
                if (!result)
                {
                    return ApiNotFound<CatalogoDTO>($"Catálogo con ID {id} no encontrado");
                }
                var updatedCatalogo = await _catalogoService.GetCatalogoByIdAsync(id);
                return ApiOk(updatedCatalogo, "Catálogo actualizado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<CatalogoDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<CatalogoDTO>("Error al actualizar el catálogo", ex);
            }
        }

        // POST: api/v1/Catalogos
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CatalogoDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CatalogoDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<CatalogoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CatalogoDTO>>> CreateCatalogo(CreateCatalogoDTO createCatalogoDto)
        {
            try
            {
                var catalogo = await _catalogoService.CreateCatalogoAsync(createCatalogoDto);
                return ApiCreated(
                    catalogo,
                    "Catálogo creado exitosamente",
                    nameof(GetCatalogo),
                    new { id = catalogo.Id }
                );
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<CatalogoDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<CatalogoDTO>("Error al crear el catálogo", ex);
            }
        }

        // DELETE: api/v1/Catalogos/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCatalogo(int id)
        {
            try
            {
                bool result = await _catalogoService.DeleteCatalogoAsync(id);
                if (!result)
                {
                    return ApiNotFound<object>($"Catálogo con ID {id} no encontrado");
                }
                return ApiOk<object>(null!, "Catálogo eliminado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar el catálogo", ex);
            }
        }
    }
}