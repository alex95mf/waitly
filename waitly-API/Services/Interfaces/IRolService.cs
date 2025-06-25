using waitly_API.Models.DTOs;

namespace waitly_API.Services.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<RolDTO>> GetAllRolesAsync();
        Task<RolDTO> GetRolByIdAsync(int id);
        Task<RolDTO> CreateRolAsync(CreateRolDTO createRolDto);
        Task<bool> UpdateRolAsync(int id, UpdateRolDTO updateRolDto);
        Task<bool> DeleteRolAsync(int id);
        Task<bool> RolExistsAsync(int id);
        Task<IEnumerable<RolDTO>> GetRolesByEmpresaIdAsync(int empresaId);
    }
}