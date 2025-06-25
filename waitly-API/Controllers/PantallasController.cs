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
    public class PantallasController : ApiControllerBase
    {
        private readonly IPantallaService _pantallaService;

        public PantallasController(IPantallaService pantallaService)
        {
            _pantallaService = pantallaService;
        }

        // GET: api/v1/Pantallas
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PantallaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PantallaDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PantallaDTO>>>> GetPantallas()
        {
            try
            {
                var pantallas = await _pantallaService.GetAllPantallasAsync();
                return ApiOk(pantallas, "Pantallas recuperadas exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<PantallaDTO>>("Error al obtener las pantallas", ex);
            }
        }

        // GET: api/v1/Pantallas/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PantallaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PantallaDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<PantallaDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PantallaDTO>>> GetPantalla(int id)
        {
            try
            {
                var pantalla = await _pantallaService.GetPantallaByIdAsync(id);

                if (pantalla == null)
                {
                    return ApiNotFound<PantallaDTO>($"Pantalla con ID {id} no encontrada");
                }

                return ApiOk(pantalla, "Pantalla recuperada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<PantallaDTO>("Error al obtener la pantalla", ex);
            }
        }

        // GET: api/v1/Pantallas/menu/5
        [HttpGet("menu/{menuId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PantallaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PantallaDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PantallaDTO>>>> GetPantallasByMenuId(int menuId)
        {
            try
            {
                var pantallas = await _pantallaService.GetPantallasByMenuIdAsync(menuId);
                return ApiOk(pantallas, "Pantallas recuperadas exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<PantallaDTO>>("Error al obtener las pantallas por menú", ex);
            }
        }

        // POST: api/v1/Pantallas
        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(ApiResponse<PantallaDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<PantallaDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PantallaDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PantallaDTO>>> CreatePantalla(CreatePantallaDTO createPantallaDto)
        {
            try
            {
                var pantalla = await _pantallaService.CreatePantallaAsync(createPantallaDto);
                return ApiCreated(
                    pantalla,
                    "Pantalla creada exitosamente",
                    nameof(GetPantalla),
                    new { id = pantalla.Id }
                );
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<PantallaDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<PantallaDTO>("Error al crear la pantalla", ex);
            }
        }

        // PUT: api/v1/Pantallas/5
        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(ApiResponse<PantallaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PantallaDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PantallaDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<PantallaDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PantallaDTO>>> UpdatePantalla(int id, UpdatePantallaDTO updatePantallaDto)
        {
            try
            {
                bool result = await _pantallaService.UpdatePantallaAsync(id, updatePantallaDto);

                if (!result)
                {
                    return ApiNotFound<PantallaDTO>($"Pantalla con ID {id} no encontrada");
                }

                var updatedPantalla = await _pantallaService.GetPantallaByIdAsync(id);
                return ApiOk(updatedPantalla, "Pantalla actualizada exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<PantallaDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<PantallaDTO>("Error al actualizar la pantalla", ex);
            }
        }

        // DELETE: api/v1/Pantallas/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeletePantalla(int id)
        {
            try
            {
                bool result = await _pantallaService.DeletePantallaAsync(id);

                if (!result)
                {
                    return ApiNotFound<object>($"Pantalla con ID {id} no encontrada");
                }

                return ApiOk<object>(null!, "Pantalla eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar la pantalla", ex);
            }
        }
    }
}