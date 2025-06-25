using iSit_API.Models.DTOs;

namespace iSit_API.Services.Interfaces
{
    public interface IPantallaService
    {
        Task<IEnumerable<PantallaDTO>> GetAllPantallasAsync();
        Task<PantallaDTO> GetPantallaByIdAsync(int id);
        Task<PantallaDTO> CreatePantallaAsync(CreatePantallaDTO createPantallaDto);
        Task<bool> UpdatePantallaAsync(int id, UpdatePantallaDTO updatePantallaDto);
        Task<bool> DeletePantallaAsync(int id);
        Task<bool> PantallaExistsAsync(int id);
        Task<IEnumerable<PantallaDTO>> GetPantallasByMenuIdAsync(int menuId);
    }
}