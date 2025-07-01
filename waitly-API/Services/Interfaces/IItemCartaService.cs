using waitly_API.Models.DTOs;

namespace waitly_API.Services.Interfaces
{
    public interface IItemCartaService
    {
        Task<IEnumerable<ItemCartaDTO>> GetAllItemsCartaAsync();
        Task<ItemCartaDTO> GetItemCartaByIdAsync(int id);
        Task<ItemCartaDTO> CreateItemCartaAsync(CreateItemCartaDTO createItemCartaDto);
        Task<bool> UpdateItemCartaAsync(int id, UpdateItemCartaDTO updateItemCartaDto);
        Task<bool> DeleteItemCartaAsync(int id);
        Task<bool> ItemCartaExistsAsync(int id);

        // Métodos específicos para filtrar items
        Task<IEnumerable<ItemCartaDTO>> GetItemsCartaByCartaIdAsync(int cartaId);
        Task<IEnumerable<ItemCartaDTO>> GetItemsCartaByCategoriaIdAsync(int categoriaId);
        Task<IEnumerable<ItemCartaDTO>> GetItemsCartaByCartaAndCategoriaAsync(int cartaId, int categoriaId);

    }
}