using waitly_API.Models.DTOs;

namespace waitly_API.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<PedidoDTO>> GetAllPedidosAsync();
        Task<PedidoDTO> GetPedidoByIdAsync(int id);
        Task<PedidoDTO> CreatePedidoAsync(CreatePedidoDTO createPedidoDto);
        Task<bool> UpdatePedidoAsync(int id, UpdatePedidoDTO updatePedidoDto);
        Task<bool> DeletePedidoAsync(int id);
        Task<bool> PedidoExistsAsync(int id);

        // Métodos específicos para filtrar pedidos
        Task<IEnumerable<PedidoDTO>> GetPedidosByEmpresaIdAsync(int empresaId);
        Task<IEnumerable<PedidoDTO>> GetPedidosByEstadoIdAsync(int estadoId);
        Task<IEnumerable<PedidoDTO>> GetPedidosByEmpresaAndEstadoAsync(int empresaId, int estadoId);
        Task<PedidoDTO> GetPedidoByCodigoAsync(string codigo);

        // Métodos para manejo de items en pedidos
        Task<bool> AddItemsToPedidoAsync(int pedidoId, List<int> itemsCartaIds);
        Task<bool> RemoveItemsFromPedidoAsync(int pedidoId, List<int> itemsCartaIds);
        Task<bool> AddItemToPedidoAsync(int pedidoId, int itemCartaId);
        Task<bool> RemoveItemFromPedidoAsync(int pedidoId, int itemCartaId);

        // Métodos para cambio de estado específicos
        Task<bool> CompletarPedidoAsync(int pedidoId);
        Task<bool> AnularPedidoAsync(int pedidoId);
        Task<bool> ReactivarPedidoAsync(int pedidoId);
    }
}