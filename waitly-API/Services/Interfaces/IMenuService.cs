using waitly_API.Models.DTOs;

namespace waitly_API.Services.Interfaces
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuDTO>> GetAllMenusAsync();
        Task<MenuDTO> GetMenuByIdAsync(int id);
        Task<MenuDTO> CreateMenuAsync(CreateMenuDTO createMenuDto);
        Task<bool> UpdateMenuAsync(int id, UpdateMenuDTO updateMenuDto);
        Task<bool> DeleteMenuAsync(int id);
        Task<bool> MenuExistsAsync(int id);

        // Obtener menús principales por empresa (solo los de primer nivel)
        Task<IEnumerable<MenuDTO>> GetMenusByEmpresaIdAsync(int empresaId);

        // Obtener árbol de menús completo para una empresa
        Task<IEnumerable<MenuTreeDTO>> GetMenuTreeByEmpresaIdAsync(int empresaId);

        // Obtener menús por usuario (filtrados por permisos)
        Task<IEnumerable<MenuUsuarioDTO>> GetMenusByUsuarioIdAsync(int usuarioId, int empresaId);
    }
}