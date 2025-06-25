using iSit_API.Models.DTOs;

namespace iSit_API.Services.Interfaces
{
    public interface IPermisoService
    {
        Task<IEnumerable<PermisoDTO>> GetAllPermisosAsync();
        Task<PermisoDTO> GetPermisoByIdAsync(int id);
        Task<PermisoDTO> CreatePermisoAsync(CreatePermisoDTO createPermisoDto);
        Task<bool> UpdatePermisoAsync(int id, UpdatePermisoDTO updatePermisoDto);
        Task<bool> DeletePermisoAsync(int id);
        Task<bool> PermisoExistsAsync(int id);
        Task<IEnumerable<PermisoDTO>> GetPermisosByUsuarioIdAsync(int usuarioId);
    }
}