using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace waitly_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartaController : ApiControllerBase
    {
        private readonly ICartaService _cartaService;

        public CartaController(ICartaService cartaService)
        {
            _cartaService = cartaService;
        }

        // GET: api/v1/Carta
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CartaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CartaDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CartaDTO>>>> GetCarta()
        {
            try
            {
                var carta = await _cartaService.GetAllCartasAsync();
                return ApiOk(carta, "Menús recuperados exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<CartaDTO>>("Error al obtener la carta", ex);
            }
        }

        // GET: api/v1/Carta/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CartaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CartaDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<CartaDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CartaDTO>>> GetCarta(int id)
        {
            try
            {
                var carta = await _cartaService.GetCartaByIdAsync(id);
                if (carta == null)
                {
                    return ApiNotFound<CartaDTO>($"Carta con ID {id} no encontrada");
                }
                return ApiOk(carta, "Carta recuperada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<CartaDTO>("Error al obtener la carta", ex);
            }
        }

        // POST: api/v1/Carta
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CartaDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CartaDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<CartaDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CartaDTO>>> CreateCarta(CreateCartaDTO createCartaDto)
        {
            try
            {
                var carta = await _cartaService.CreateCartaAsync(createCartaDto);
                return ApiCreated(
                    carta,
                    "Carta creada exitosamente",
                    nameof(GetCarta)
                );
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<CartaDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<CartaDTO>("Error al crear la carta", ex);
            }
        }

        // PUT: api/v1/Carta/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CartaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CartaDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<CartaDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<CartaDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CartaDTO>>> UpdateMenu(int id, UpdateCartaDTO updateCartaDto)
        {
            try
            {
                bool result = await _cartaService.UpdateCartaAsync(id, updateCartaDto);
                if (!result)
                {
                    return ApiNotFound<CartaDTO>($"Carta con ID {id} no encontrada");
                }
                var updatedCarta = await _cartaService.GetCartaByIdAsync(id);
                return ApiOk(updatedCarta, "Carta actualizada exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<CartaDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<CartaDTO>("Error al actualizar la carta", ex);
            }
        }

        // DELETE: api/v1/Carta/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCarta(int id)
        {
            try
            {
                bool result = await _cartaService.DeleteCartaAsync(id);
                if (!result)
                {
                    return ApiNotFound<object>($"Carta con ID {id} no encontrada");
                }
                return ApiOk<object>(null!, "Carta eliminado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar la carta", ex);
            }
        }

        // REVISAR A PARTIR DE AQUÍ:

        //// GET: api/v1/Carta/empresa/{empresaId}
        //[HttpGet("empresa/{empresaId}")]
        //[Authorize]
        //[ProducesResponseType(typeof(ApiResponse<IEnumerable<CartaDTO>>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse<IEnumerable<CartaDTO>>), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ApiResponse<IEnumerable<CartaDTO>>), StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<ApiResponse<IEnumerable<CartaDTO>>>> GetMenusByEmpresa(int empresaId)
        //{
        //    try
        //    {
        //        var carta = await _cartaService.GetMenusByEmpresaIdAsync(empresaId);
        //        return ApiOk(carta, $"Menús de la empresa con ID {empresaId} recuperados exitosamente");
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return ApiBadRequest<IEnumerable<CartaDTO>>(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiServerError<IEnumerable<CartaDTO>>($"Error al obtener los menús de la empresa con ID {empresaId}", ex);
        //    }
        //}

        //// GET: api/v1/Carta/empresa/{empresaId}/tree
        //[HttpGet("empresa/{empresaId}/tree")]
        //[Authorize]
        //[ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuTreeDTO>>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuTreeDTO>>), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuTreeDTO>>), StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<ApiResponse<IEnumerable<MenuTreeDTO>>>> GetMenuTreeByEmpresa(int empresaId)
        //{
        //    try
        //    {
        //        var menuTree = await _cartaService.GetMenuTreeByEmpresaIdAsync(empresaId);
        //        return ApiOk(menuTree, $"Árbol de menús de la empresa con ID {empresaId} recuperado exitosamente");
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return ApiBadRequest<IEnumerable<MenuTreeDTO>>(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiServerError<IEnumerable<MenuTreeDTO>>($"Error al obtener el árbol de menús de la empresa con ID {empresaId}", ex);
        //    }
        //}

        //// GET: api/v1/Carta/usuario/{usuarioId}/empresa/{empresaId}
        //[HttpGet("usuario/{usuarioId}/empresa/{empresaId}")]
        //[Authorize]
        //[ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuUsuarioDTO>>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuUsuarioDTO>>), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuUsuarioDTO>>), StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<ApiResponse<IEnumerable<MenuUsuarioDTO>>>> GetMenusByUsuario(int usuarioId, int empresaId)
        //{
        //    try
        //    {
        //        var carta = await _cartaService.GetMenusByUsuarioIdAsync(usuarioId, empresaId);
        //        return ApiOk(carta, $"Menús del usuario con ID {usuarioId} en la empresa con ID {empresaId} recuperados exitosamente");
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return ApiBadRequest<IEnumerable<MenuUsuarioDTO>>(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiServerError<IEnumerable<MenuUsuarioDTO>>($"Error al obtener los menús del usuario con ID {usuarioId} en la empresa con ID {empresaId}", ex);
        //    }
        //}
    }
}