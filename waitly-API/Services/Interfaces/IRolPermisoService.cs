using waitly_API.Models.DTOs;

namespace waitly_API.Services.Interfaces
{
    public interface IRolPermisoService
    {
        Task<IEnumerable<PermisoDTO>> GetPermisosByRolIdAsync(int rolId);
        Task<IEnumerable<RolDTO>> GetRolesByPermisoIdAsync(int permisoId);
        Task AsignarPermisosARolAsync(int rolId, List<int> permisoIds);
        Task QuitarPermisosDeRolAsync(int rolId, List<int> permisoIds);
    }
}