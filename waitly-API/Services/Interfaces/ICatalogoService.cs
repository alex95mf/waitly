using iSit_API.Models.DTOs;

namespace iSit_API.Services.Interfaces
{
    public interface ICatalogoService
    {
        Task<IEnumerable<CatalogoDTO>> GetAllCatalogosAsync();
        Task<CatalogoDTO> GetCatalogoByIdAsync(int id);
        Task<IEnumerable<CatalogoDTO>> GetCatalogosByPadreIdAsync(int? padreId);
        Task<CatalogoDTO> CreateCatalogoAsync(CreateCatalogoDTO createCatalogoDto);
        Task<bool> UpdateCatalogoAsync(int id, UpdateCatalogoDTO updateCatalogoDto);
        Task<bool> DeleteCatalogoAsync(int id);
        Task<bool> CatalogoExistsAsync(int id);
    }
}