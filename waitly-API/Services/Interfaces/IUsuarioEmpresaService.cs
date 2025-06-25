using waitly_API.Models.DTOs;

namespace waitly_API.Services.Interfaces
{
    public interface IUsuarioEmpresaService
    {
        Task<IEnumerable<EmpresaDTO>> GetEmpresasByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<UsuarioDTO>> GetUsuariosByEmpresaIdAsync(int empresaId);
        Task AsignarEmpresasAUsuarioAsync(int usuarioId, List<int> empresaIds);
        Task QuitarEmpresasDeUsuarioAsync(int usuarioId, List<int> empresaIds);
    }
}
