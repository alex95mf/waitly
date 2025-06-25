using iSit_API.Models;
using iSit_API.Models.DTOs;

namespace iSit_API.Services.Interfaces
{
    public interface IUsuarioService
    {

        Task<IEnumerable<UsuarioDTO>> GetAllUsuariosAsync();
        Task<UsuarioDTO> GetUsuarioByIdAsync(int id);
        Task<AuthResponseDTO> CreateUsuarioAsync(CreateUsuarioDTO createUsuarioDto);
        Task<bool> UpdateUsuarioAsync(int id, UpdateUsuarioDTO updateUsuarioDto);
        Task<bool> DeleteUsuarioAsync(int id);
        Task<bool> UsuarioExistsAsync(int id);
        Task<AuthResponseDTO> AuthenticateAsync(LoginUsuarioDTO loginDto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO changePasswordDto);
        AuthResponseDTO GenerateTokenAsync(UsuarioDTO usuario);
    }
}
