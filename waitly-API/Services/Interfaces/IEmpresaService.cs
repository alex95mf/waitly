using waitly_API.Models;
using waitly_API.Models.DTOs;

namespace waitly_API.Services.Interfaces
{
    public interface IEmpresaService
    {
        Task<IEnumerable<EmpresaDTO>> GetAllEmpresasAsync();
        Task<EmpresaDTO> GetEmpresaByIdAsync(int id);
        Task<EmpresaDTO> CreateEmpresaAsync(CreateEmpresaDTO createEmpresaDto);
        Task<bool> UpdateEmpresaAsync(int id, UpdateEmpresaDTO updateEmpresaDto);
        Task<bool> DeleteEmpresaAsync(int id);
        Task<bool> EmpresaExistsAsync(int id);
    }
}