using iSit_API.Models.DTOs;
using iSit_API.Models.Entities;
using iSit_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iSit_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MenusController : ApiControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        // GET: api/v1/Menus
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MenuDTO>>>> GetMenus()
        {
            try
            {
                var menus = await _menuService.GetAllMenusAsync();
                return ApiOk(menus, "Menús recuperados exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<MenuDTO>>("Error al obtener los menús", ex);
            }
        }

        // GET: api/v1/Menus/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<MenuDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<MenuDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<MenuDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<MenuDTO>>> GetMenu(int id)
        {
            try
            {
                var menu = await _menuService.GetMenuByIdAsync(id);
                if (menu == null)
                {
                    return ApiNotFound<MenuDTO>($"Menú con ID {id} no encontrado");
                }
                return ApiOk(menu, "Menú recuperado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiServerError<MenuDTO>("Error al obtener el menú", ex);
            }
        }

        // POST: api/v1/Menus
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<MenuDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<MenuDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<MenuDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<MenuDTO>>> CreateMenu(CreateMenuDTO createMenuDto)
        {
            try
            {
                var menu = await _menuService.CreateMenuAsync(createMenuDto);
                return ApiCreated(
                    menu,
                    "Menú creado exitosamente",
                    nameof(GetMenu),
                    new { id = menu.Id }
                );
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<MenuDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<MenuDTO>("Error al crear el menú", ex);
            }
        }

        // PUT: api/v1/Menus/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<MenuDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<MenuDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<MenuDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<MenuDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<MenuDTO>>> UpdateMenu(int id, UpdateMenuDTO updateMenuDto)
        {
            try
            {
                bool result = await _menuService.UpdateMenuAsync(id, updateMenuDto);
                if (!result)
                {
                    return ApiNotFound<MenuDTO>($"Menú con ID {id} no encontrado");
                }
                var updatedMenu = await _menuService.GetMenuByIdAsync(id);
                return ApiOk(updatedMenu, "Menú actualizado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<MenuDTO>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<MenuDTO>("Error al actualizar el menú", ex);
            }
        }

        // DELETE: api/v1/Menus/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMenu(int id)
        {
            try
            {
                bool result = await _menuService.DeleteMenuAsync(id);
                if (!result)
                {
                    return ApiNotFound<object>($"Menú con ID {id} no encontrado");
                }
                return ApiOk<object>(null!, "Menú eliminado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<object>("Error al eliminar el menú", ex);
            }
        }

        // GET: api/v1/Menus/empresa/{empresaId}
        [HttpGet("empresa/{empresaId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MenuDTO>>>> GetMenusByEmpresa(int empresaId)
        {
            try
            {
                var menus = await _menuService.GetMenusByEmpresaIdAsync(empresaId);
                return ApiOk(menus, $"Menús de la empresa con ID {empresaId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<MenuDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<MenuDTO>>($"Error al obtener los menús de la empresa con ID {empresaId}", ex);
            }
        }

        // GET: api/v1/Menus/empresa/{empresaId}/tree
        [HttpGet("empresa/{empresaId}/tree")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuTreeDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuTreeDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuTreeDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MenuTreeDTO>>>> GetMenuTreeByEmpresa(int empresaId)
        {
            try
            {
                var menuTree = await _menuService.GetMenuTreeByEmpresaIdAsync(empresaId);
                return ApiOk(menuTree, $"Árbol de menús de la empresa con ID {empresaId} recuperado exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<MenuTreeDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<MenuTreeDTO>>($"Error al obtener el árbol de menús de la empresa con ID {empresaId}", ex);
            }
        }

        // GET: api/v1/Menus/usuario/{usuarioId}/empresa/{empresaId}
        [HttpGet("usuario/{usuarioId}/empresa/{empresaId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuUsuarioDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuUsuarioDTO>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MenuUsuarioDTO>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MenuUsuarioDTO>>>> GetMenusByUsuario(int usuarioId, int empresaId)
        {
            try
            {
                var menus = await _menuService.GetMenusByUsuarioIdAsync(usuarioId, empresaId);
                return ApiOk(menus, $"Menús del usuario con ID {usuarioId} en la empresa con ID {empresaId} recuperados exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                return ApiBadRequest<IEnumerable<MenuUsuarioDTO>>(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiServerError<IEnumerable<MenuUsuarioDTO>>($"Error al obtener los menús del usuario con ID {usuarioId} en la empresa con ID {empresaId}", ex);
            }
        }
    }
}