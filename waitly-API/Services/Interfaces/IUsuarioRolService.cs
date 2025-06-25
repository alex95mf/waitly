using iSit_API.Models.DTOs;

namespace iSit_API.Services.Interfaces
{
    public interface IUsuarioRolService
    {
        Task<IEnumerable<RolDTO>> GetRolesByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<UsuarioDTO>> GetUsuariosByRolIdAsync(int rolId);
        Task AsignarRolesAUsuarioAsync(int usuarioId, List<int> rolIds);
        Task QuitarRolesDeUsuarioAsync(int usuarioId, List<int> rolIds);
    }
}