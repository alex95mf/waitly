using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace waitly_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ItemCartaController : ApiControllerBase
    {
        private readonly IItemCartaService _itemCartaService;

        public ItemCartaController(IItemCartaService itemCartaService)
        {
            _itemCartaService = itemCartaService;
        }

        // GET: api/v1/ItemCarta
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemCartaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemCartaDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ItemCartaDTO>>>> GetItemsCarta()
        {
            try
            {
                var itemsCarta = await _itemCartaService.GetAllItemsCartaAsync();
                return ApiOk(itemsCarta, "Items de carta recuperados exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<ItemCartaDTO>>("Error al obtener los items de carta", ex);
            }
        }

        // GET: api/v1/ItemCarta/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<ItemCartaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ItemCartaDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<ItemCartaDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<ItemCartaDTO>>> GetItemCarta(int id)
        {
            try
            {
                var itemCarta = await _itemCartaService.GetItemCartaByIdAsync(id);
                if (itemCarta == null)
                {
                    return ApiNotFound<ItemCartaDTO>($"Item de carta con ID {id} no encontrado");
                }
                return ApiOk(itemCarta, "Item de carta recuperado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<ItemCartaDTO>("Error al obtener el item de carta", ex);
            }
        }

        // POST: api/v1/ItemCarta
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<ItemCartaDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<ItemCartaDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<ItemCartaDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<ItemCartaDTO>>> CreateItemCarta(CreateItemCartaDTO createItemCartaDto)
        {
            try
            {
                var itemCarta = await _itemCartaService.CreateItemCartaAsync(createItemCartaDto);
                return ApiCreated(
                    itemCarta,
                    "Item de carta creado exitosamente",
                    nameof(GetItemCarta),
                    new { id = itemCarta.Id }
                );
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<ItemCartaDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<ItemCartaDTO>("Error al crear el item de carta", ex);
            }
        }

        // PUT: api/v1/ItemCarta/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<ItemCartaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ItemCartaDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<ItemCartaDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<ItemCartaDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<ItemCartaDTO>>> UpdateItemCarta(int id, UpdateItemCartaDTO updateItemCartaDto)
        {
            try
            {
                bool result = await _itemCartaService.UpdateItemCartaAsync(id, updateItemCartaDto);
                if (!result)
                {
                    return ApiNotFound<ItemCartaDTO>($"Item de carta con ID {id} no encontrado");
                }
                var updatedItemCarta = await _itemCartaService.GetItemCartaByIdAsync(id);
                return ApiOk(updatedItemCarta, "Item de carta actualizado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<ItemCartaDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<ItemCartaDTO>("Error al actualizar el item de carta", ex);
            }
        }

        // DELETE: api/v1/ItemCarta/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteItemCarta(int id)
        {
            try
            {
                bool result = await _itemCartaService.DeleteItemCartaAsync(id);
                if (!result)
                {
                    return ApiNotFound<object>($"Item de carta con ID {id} no encontrado");
                }
                return ApiOk<object>(null!, "Item de carta eliminado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar el item de carta", ex);
            }
        }

        // GET: api/v1/ItemCarta/carta/5
        [HttpGet("carta/{cartaId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemCartaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemCartaDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemCartaDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ItemCartaDTO>>>> GetItemsCartaByCarta(int cartaId)
        {
            try
            {
                var itemsCarta = await _itemCartaService.GetItemsCartaByCartaIdAsync(cartaId);
                return ApiOk(itemsCarta, $"Items de la carta con ID {cartaId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<ItemCartaDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<ItemCartaDTO>>($"Error al obtener los items de la carta con ID {cartaId}", ex);
            }
        }

        // GET: api/v1/ItemCarta/categoria/5
        [HttpGet("categoria/{categoriaId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemCartaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemCartaDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemCartaDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ItemCartaDTO>>>> GetItemsCartaByCategoria(int categoriaId)
        {
            try
            {
                var itemsCarta = await _itemCartaService.GetItemsCartaByCategoriaIdAsync(categoriaId);
                return ApiOk(itemsCarta, $"Items de la categoría con ID {categoriaId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<ItemCartaDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<ItemCartaDTO>>($"Error al obtener los items de la categoría con ID {categoriaId}", ex);
            }
        }

        // GET: api/v1/ItemCarta/carta/5/categoria/3
        [HttpGet("carta/{cartaId}/categoria/{categoriaId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemCartaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemCartaDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemCartaDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ItemCartaDTO>>>> GetItemsCartaByCartaAndCategoria(int cartaId, int categoriaId)
        {
            try
            {
                var itemsCarta = await _itemCartaService.GetItemsCartaByCartaAndCategoriaAsync(cartaId, categoriaId);
                return ApiOk(itemsCarta, $"Items de la carta {cartaId} y categoría {categoriaId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<ItemCartaDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<ItemCartaDTO>>($"Error al obtener los items de la carta {cartaId} y categoría {categoriaId}", ex);
            }
        }
    }
}