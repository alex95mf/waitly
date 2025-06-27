using waitly_API.Models.DTOs;

namespace waitly_API.Services.Interfaces
{
    public interface ICartaService
    {
        Task<IEnumerable<CartaDTO>> GetAllCartasAsync();
        Task<CartaDTO> GetCartaByIdAsync(int id);
        Task<CartaDTO> CreateCartaAsync(CreateCartaDTO createMenuDto);
        Task<bool> UpdateCartaAsync(int id, UpdateCartaDTO updateMenuDto);
        Task<bool> DeleteCartaAsync(int id);
        Task<bool> CartaExistsAsync(int id);

        // Obtener menús principales por empresa (solo los de primer nivel)
        Task<IEnumerable<CartaDTO>> GetCartasByEmpresaIdAsync(int empresaId);

    }
}