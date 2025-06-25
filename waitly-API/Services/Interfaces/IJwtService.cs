using waitly_API.Models.DTOs;

namespace waitly_API.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UsuarioDTO usuario);
    }
}
