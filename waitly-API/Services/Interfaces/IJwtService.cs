using iSit_API.Models.DTOs;

namespace iSit_API.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UsuarioDTO usuario);
    }
}
