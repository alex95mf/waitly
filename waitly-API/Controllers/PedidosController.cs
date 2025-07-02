using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace waitly_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PedidosController : ApiControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        // GET: api/v1/Pedidos
        [HttpGet]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PedidoDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PedidoDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PedidoDTO>>>> GetPedidos()
        {
            try
            {
                var pedidos = await _pedidoService.GetAllPedidosAsync();
                return ApiOk(pedidos, "Pedidos recuperados exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<PedidoDTO>>("Error al obtener los pedidos", ex);
            }
        }

        // GET: api/v1/Pedidos/5
        [HttpGet("{id}")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PedidoDTO>>> GetPedido(int id)
        {
            try
            {
                var pedido = await _pedidoService.GetPedidoByIdAsync(id);
                if (pedido == null)
                {
                    return ApiNotFound<PedidoDTO>($"Pedido con ID {id} no encontrado");
                }
                return ApiOk(pedido, "Pedido recuperado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<PedidoDTO>("Error al obtener el pedido", ex);
            }
        }

        // GET: api/v1/Pedidos/codigo/PED001
        [HttpGet("codigo/{codigo}")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PedidoDTO>>> GetPedidoByCodigo(string codigo)
        {
            try
            {
                var pedido = await _pedidoService.GetPedidoByCodigoAsync(codigo);
                if (pedido == null)
                {
                    return ApiNotFound<PedidoDTO>($"Pedido con código {codigo} no encontrado");
                }
                return ApiOk(pedido, "Pedido recuperado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<PedidoDTO>("Error al obtener el pedido", ex);
            }
        }

        // POST: api/v1/Pedidos
        [HttpPost]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PedidoDTO>>> CreatePedido(CreatePedidoDTO createPedidoDto)
        {
            try
            {
                var pedido = await _pedidoService.CreatePedidoAsync(createPedidoDto);
                return ApiCreated(
                    pedido,
                    "Pedido creado exitosamente",
                    nameof(GetPedido),
                    new { id = pedido.Id }
                );
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<PedidoDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<PedidoDTO>("Error al crear el pedido", ex);
            }
        }

        // PUT: api/v1/Pedidos/5
        [HttpPut("{id}")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PedidoDTO>>> UpdatePedido(int id, UpdatePedidoDTO updatePedidoDto)
        {
            try
            {
                bool result = await _pedidoService.UpdatePedidoAsync(id, updatePedidoDto);
                if (!result)
                {
                    return ApiNotFound<PedidoDTO>($"Pedido con ID {id} no encontrado");
                }
                var updatedPedido = await _pedidoService.GetPedidoByIdAsync(id);
                return ApiOk(updatedPedido, "Pedido actualizado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<PedidoDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<PedidoDTO>("Error al actualizar el pedido", ex);
            }
        }

        // DELETE: api/v1/Pedidos/5
        [HttpDelete("{id}")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeletePedido(int id)
        {
            try
            {
                bool result = await _pedidoService.DeletePedidoAsync(id);
                if (!result)
                {
                    return ApiNotFound<object>($"Pedido con ID {id} no encontrado");
                }
                return ApiOk<object>(null!, "Pedido eliminado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar el pedido", ex);
            }
        }

        // GET: api/v1/Pedidos/empresa/5
        [HttpGet("empresa/{empresaId}")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PedidoDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PedidoDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PedidoDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PedidoDTO>>>> GetPedidosByEmpresa(int empresaId)
        {
            try
            {
                var pedidos = await _pedidoService.GetPedidosByEmpresaIdAsync(empresaId);
                return ApiOk(pedidos, $"Pedidos de la empresa con ID {empresaId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<PedidoDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<PedidoDTO>>($"Error al obtener los pedidos de la empresa con ID {empresaId}", ex);
            }
        }

        // GET: api/v1/Pedidos/estado/3
        [HttpGet("estado/{estadoId}")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PedidoDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PedidoDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PedidoDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PedidoDTO>>>> GetPedidosByEstado(int estadoId)
        {
            try
            {
                var pedidos = await _pedidoService.GetPedidosByEstadoIdAsync(estadoId);
                return ApiOk(pedidos, $"Pedidos con estado ID {estadoId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<PedidoDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<PedidoDTO>>($"Error al obtener los pedidos con estado ID {estadoId}", ex);
            }
        }

        // GET: api/v1/Pedidos/empresa/5/estado/3
        [HttpGet("empresa/{empresaId}/estado/{estadoId}")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PedidoDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PedidoDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PedidoDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PedidoDTO>>>> GetPedidosByEmpresaAndEstado(int empresaId, int estadoId)
        {
            try
            {
                var pedidos = await _pedidoService.GetPedidosByEmpresaAndEstadoAsync(empresaId, estadoId);
                return ApiOk(pedidos, $"Pedidos de la empresa {empresaId} con estado {estadoId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<PedidoDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<PedidoDTO>>($"Error al obtener los pedidos de la empresa {empresaId} con estado {estadoId}", ex);
            }
        }

        // POST: api/v1/Pedidos/5/items (múltiples items)
        [HttpPost("{pedidoId}/items")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> AddItemsToPedido(int pedidoId, AddItemsToPedidoDTO addItemsDto)
        {
            try
            {
                bool result = await _pedidoService.AddItemsToPedidoAsync(pedidoId, addItemsDto.ItemsCartaIds);
                if (!result)
                {
                    return ApiNotFound<object>("Pedido no encontrado");
                }
                return ApiOk<object>(null!, $"{addItemsDto.ItemsCartaIds.Count} items agregados al pedido exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al agregar items al pedido", ex);
            }
        }

        // DELETE: api/v1/Pedidos/5/items (múltiples items)
        [HttpDelete("{pedidoId}/items")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> RemoveItemsFromPedido(int pedidoId, RemoveItemsFromPedidoDTO removeItemsDto)
        {
            try
            {
                bool result = await _pedidoService.RemoveItemsFromPedidoAsync(pedidoId, removeItemsDto.ItemsCartaIds);
                if (!result)
                {
                    return ApiNotFound<object>("Items no encontrados en el pedido");
                }
                return ApiOk<object>(null!, $"{removeItemsDto.ItemsCartaIds.Count} items removidos del pedido exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al remover items del pedido", ex);
            }
        }

        // POST: api/v1/Pedidos/5/items/3 (un solo item - mantenido por compatibilidad)
        [HttpPost("{pedidoId}/items/{itemCartaId}")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> AddItemToPedido(int pedidoId, int itemCartaId)
        {
            try
            {
                bool result = await _pedidoService.AddItemToPedidoAsync(pedidoId, itemCartaId);
                if (!result)
                {
                    return ApiNotFound<object>("Pedido o item no encontrado");
                }
                return ApiOk<object>(null!, "Item agregado al pedido exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al agregar item al pedido", ex);
            }
        }

        // DELETE: api/v1/Pedidos/5/items/3 (un solo item - mantenido por compatibilidad)
        [HttpDelete("{pedidoId}/items/{itemCartaId}")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> RemoveItemFromPedido(int pedidoId, int itemCartaId)
        {
            try
            {
                bool result = await _pedidoService.RemoveItemFromPedidoAsync(pedidoId, itemCartaId);
                if (!result)
                {
                    return ApiNotFound<object>("Item no encontrado en el pedido");
                }
                return ApiOk<object>(null!, "Item removido del pedido exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al remover item del pedido", ex);
            }
        }

        // POST: api/v1/Pedidos/5/completar
        [HttpPost("{id}/completar")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PedidoDTO>>> CompletarPedido(int id)
        {
            try
            {
                bool result = await _pedidoService.CompletarPedidoAsync(id);
                if (!result)
                {
                    return ApiNotFound<PedidoDTO>($"Pedido con ID {id} no encontrado");
                }
                var pedidoActualizado = await _pedidoService.GetPedidoByIdAsync(id);
                return ApiOk(pedidoActualizado, "Pedido completado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<PedidoDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<PedidoDTO>("Error al completar el pedido", ex);
            }
        }

        // POST: api/v1/Pedidos/5/anular
        [HttpPost("{id}/anular")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PedidoDTO>>> AnularPedido(int id)
        {
            try
            {
                bool result = await _pedidoService.AnularPedidoAsync(id);
                if (!result)
                {
                    return ApiNotFound<PedidoDTO>($"Pedido con ID {id} no encontrado");
                }
                var pedidoActualizado = await _pedidoService.GetPedidoByIdAsync(id);
                return ApiOk(pedidoActualizado, "Pedido anulado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<PedidoDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<PedidoDTO>("Error al anular el pedido", ex);
            }
        }

        // POST: api/v1/Pedidos/5/reactivar
        [HttpPost("{id}/reactivar")]
       // [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<PedidoDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PedidoDTO>>> ReactivarPedido(int id)
        {
            try
            {
                bool result = await _pedidoService.ReactivarPedidoAsync(id);
                if (!result)
                {
                    return ApiNotFound<PedidoDTO>($"Pedido con ID {id} no encontrado");
                }
                var pedidoActualizado = await _pedidoService.GetPedidoByIdAsync(id);
                return ApiOk(pedidoActualizado, "Pedido reactivado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<PedidoDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<PedidoDTO>("Error al reactivar el pedido", ex);
            }
        }
    }
}