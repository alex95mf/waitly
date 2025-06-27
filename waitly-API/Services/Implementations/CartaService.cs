//using waitly_API.Models.DTOs;
//using waitly_API.Models.Entities;
//using waitly_API.Services.Interfaces;
//using waitly_API.Data;
//using Microsoft.EntityFrameworkCore;

//namespace waitly_API.Services.Implementations
//{
//    public class MenuService : IMenuService
//    {
//        private readonly ApplicationDbContext _context;

//        public MenuService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<MenuDTO>> GetAllMenusAsync()
//        {
//            var menus = await _context.Menus
//                .Include(m => m.Empresa)
//                .Include(m => m.Tipo)
//                .Include(m => m.Permiso)
//                .Include(m => m.MenuPadre)
//                .Where(m => !m.Removido)
//                .ToListAsync();

//            return menus.Select(MapToMenuDTO);
//        }

//        public async Task<MenuDTO> GetMenuByIdAsync(int id)
//        {
//            var menu = await _context.Menus
//                .Include(m => m.Empresa)
//                .Include(m => m.Tipo)
//                .Include(m => m.Permiso)
//                .Include(m => m.MenuPadre)
//                .Include(m => m.SubMenus.Where(sm => !sm.Removido))
//                .FirstOrDefaultAsync(m => m.Id == id && !m.Removido);

//            if (menu == null)
//                return null;

//            var menuDto = MapToMenuDTO(menu);

//            // Añadir submenús
//            if (menu.SubMenus != null && menu.SubMenus.Any())
//            {
//                menuDto.SubMenus = menu.SubMenus.Select(MapToMenuDTO).ToList();
//            }

//            return menuDto;
//        }

//        public async Task<MenuDTO> CreateMenuAsync(CreateMenuDTO createMenuDto)
//        {
//            // Validar que la empresa existe
//            var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == createMenuDto.IdEmpresa && !e.Removido);
//            if (!empresaExists)
//            {
//                throw new InvalidOperationException($"Empresa con ID {createMenuDto.IdEmpresa} no encontrada");
//            }

//            // Validar que el tipo existe
//            var tipoExists = await _context.Catalogos.AnyAsync(c => c.Id == createMenuDto.IdTipo && !c.Removido);
//            if (!tipoExists)
//            {
//                throw new InvalidOperationException($"Tipo de menú con ID {createMenuDto.IdTipo} no encontrado");
//            }

//            // Validar que el permiso existe, solo si VerificaPermiso es true y se especifica un permiso
//            if (createMenuDto.VerificaPermiso && createMenuDto.IdPermiso.HasValue)
//            {
//                var permisoExists = await _context.Permisos.AnyAsync(p => p.Id == createMenuDto.IdPermiso.Value && !p.Removido);
//                if (!permisoExists)
//                {
//                    throw new InvalidOperationException($"Permiso con ID {createMenuDto.IdPermiso.Value} no encontrado");
//                }
//            }

//            // Validar que el menú padre existe, si se especifica y es mayor que 0
//            if (createMenuDto.IdPadre.HasValue && createMenuDto.IdPadre.Value > 0)
//            {
//                var menuPadreExists = await _context.Menus.AnyAsync(m => m.Id == createMenuDto.IdPadre.Value && !m.Removido);
//                if (!menuPadreExists)
//                {
//                    throw new InvalidOperationException($"Menú padre con ID {createMenuDto.IdPadre.Value} no encontrado");
//                }
//            }

//            var menu = new Menu
//            {
//                Nombre = createMenuDto.Nombre,
//                Nemonico = createMenuDto.Nemonico,
//                Descripcion = createMenuDto.Descripcion,
//                VerificaPermiso = createMenuDto.VerificaPermiso,
//                Orden = createMenuDto.Orden,
//                IdPadre = createMenuDto.IdPadre > 0 ? createMenuDto.IdPadre : null,
//                Url = createMenuDto.Url,
//                IdTipo = createMenuDto.IdTipo,
//                IdEmpresa = createMenuDto.IdEmpresa,
//                // Asignar IdPermiso solo si VerificaPermiso es true
//                IdPermiso = createMenuDto.VerificaPermiso ? createMenuDto.IdPermiso : null,
//                FechaCreacion = DateTime.Now,
//                IpCreacion = "IP.PRUEBA",
//                Removido = false
//            };

//            _context.Menus.Add(menu);
//            await _context.SaveChangesAsync();

//            return await GetMenuByIdAsync(menu.Id);
//        }

//        public async Task<bool> UpdateMenuAsync(int id, UpdateMenuDTO updateMenuDto)
//        {
//            var menu = await _context.Menus.FindAsync(id);

//            if (menu == null || menu.Removido)
//            {
//                return false;
//            }

//            // Validar que la empresa existe, si se especifica
//            if (updateMenuDto.IdEmpresa.HasValue)
//            {
//                var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == updateMenuDto.IdEmpresa.Value && !e.Removido);
//                if (!empresaExists)
//                {
//                    throw new InvalidOperationException($"Empresa con ID {updateMenuDto.IdEmpresa.Value} no encontrada");
//                }
//                menu.IdEmpresa = updateMenuDto.IdEmpresa.Value;
//            }

//            // Validar que el tipo existe, si se especifica
//            if (updateMenuDto.IdTipo.HasValue)
//            {
//                var tipoExists = await _context.Catalogos.AnyAsync(c => c.Id == updateMenuDto.IdTipo.Value && !c.Removido);
//                if (!tipoExists)
//                {
//                    throw new InvalidOperationException($"Tipo de menú con ID {updateMenuDto.IdTipo.Value} no encontrado");
//                }
//                menu.IdTipo = updateMenuDto.IdTipo.Value;
//            }

//            // Actualizar VerificaPermiso primero si se proporciona
//            if (updateMenuDto.VerificaPermiso.HasValue)
//            {
//                menu.VerificaPermiso = updateMenuDto.VerificaPermiso.Value;
//            }

//            // Validar que el permiso existe, solo si VerificaPermiso es true y se especifica un permiso
//            if (menu.VerificaPermiso && updateMenuDto.IdPermiso.HasValue)
//            {
//                var permisoExists = await _context.Permisos.AnyAsync(p => p.Id == updateMenuDto.IdPermiso.Value && !p.Removido);
//                if (!permisoExists)
//                {
//                    throw new InvalidOperationException($"Permiso con ID {updateMenuDto.IdPermiso.Value} no encontrado");
//                }
//                menu.IdPermiso = updateMenuDto.IdPermiso.Value;
//            }
//            else if (!menu.VerificaPermiso)
//            {
//                // Si VerificaPermiso es false, establecer IdPermiso a null
//                menu.IdPermiso = null;
//            }

//            // Validar que el menú padre existe, si se especifica
//            if (updateMenuDto.IdPadre.HasValue)
//            {
//                // Validar que no se esté asignando a sí mismo como padre
//                if (updateMenuDto.IdPadre.Value == id)
//                {
//                    throw new InvalidOperationException("Un menú no puede ser su propio padre");
//                }

//                if (updateMenuDto.IdPadre.Value > 0)
//                {
//                    var menuPadreExists = await _context.Menus.AnyAsync(m => m.Id == updateMenuDto.IdPadre.Value && !m.Removido);
//                    if (!menuPadreExists)
//                    {
//                        throw new InvalidOperationException($"Menú padre con ID {updateMenuDto.IdPadre.Value} no encontrado");
//                    }
//                    menu.IdPadre = updateMenuDto.IdPadre.Value;
//                }
//                else
//                {
//                    menu.IdPadre = null;
//                }
//            }

//            // Actualizar propiedades del menú
//            if (!string.IsNullOrEmpty(updateMenuDto.Nombre))
//                menu.Nombre = updateMenuDto.Nombre;

//            if (!string.IsNullOrEmpty(updateMenuDto.Nemonico))
//                menu.Nemonico = updateMenuDto.Nemonico;

//            if (!string.IsNullOrEmpty(updateMenuDto.Descripcion))
//                menu.Descripcion = updateMenuDto.Descripcion;

//            if (!string.IsNullOrEmpty(updateMenuDto.Url))
//                menu.Url = updateMenuDto.Url;

//            if (updateMenuDto.Orden.HasValue)
//                menu.Orden = updateMenuDto.Orden.Value;

//            menu.FechaModificacion = DateTime.Now;
//            menu.IpModificacion = "IP.PRUEBA";

//            try
//            {
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!await MenuExistsAsync(id))
//                {
//                    return false;
//                }
//                else
//                {
//                    throw;
//                }
//            }
//        }

//        public async Task<bool> DeleteMenuAsync(int id)
//        {
//            var menu = await _context.Menus
//                .Include(m => m.SubMenus)
//                .FirstOrDefaultAsync(m => m.Id == id && !m.Removido);

//            if (menu == null)
//            {
//                return false;
//            }

//            // Verificar si tiene submenús activos
//            if (menu.SubMenus != null && menu.SubMenus.Any(sm => !sm.Removido))
//            {
//                throw new InvalidOperationException("No se puede eliminar un menú que tiene submenús activos");
//            }

//            // Marcar como eliminado (soft delete)
//            menu.Removido = true;
//            menu.FechaEliminacion = DateTime.Now;
//            menu.IpEliminacion = "IP.PRUEBA";

//            await _context.SaveChangesAsync();
//            return true;
//        }

//        public async Task<bool> MenuExistsAsync(int id)
//        {
//            return await _context.Menus.AnyAsync(e => e.Id == id && !e.Removido);
//        }

//        public async Task<IEnumerable<MenuDTO>> GetMenusByEmpresaIdAsync(int empresaId)
//        {
//            var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == empresaId && !e.Removido);
//            if (!empresaExists)
//            {
//                throw new InvalidOperationException($"Empresa con ID {empresaId} no encontrada");
//            }

//            var menus = await _context.Menus
//                .Include(m => m.Tipo)
//                .Include(m => m.Permiso)
//                .Include(m => m.SubMenus.Where(sm => !sm.Removido))
//                .Where(m => m.IdEmpresa == empresaId && !m.Removido && m.IdPadre == null)
//                .OrderBy(m => m.Orden)
//                .ToListAsync();

//            var menuDtos = new List<MenuDTO>();
//            foreach (var menu in menus)
//            {
//                var menuDto = MapToMenuDTO(menu);

//                // Añadir submenús recursivamente
//                if (menu.SubMenus != null && menu.SubMenus.Any())
//                {
//                    menuDto.SubMenus = menu.SubMenus
//                        .OrderBy(sm => sm.Orden)
//                        .Select(MapToMenuDTO)
//                        .ToList();
//                }

//                menuDtos.Add(menuDto);
//            }

//            return menuDtos;
//        }

//        public async Task<IEnumerable<MenuTreeDTO>> GetMenuTreeByEmpresaIdAsync(int empresaId)
//        {
//            var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == empresaId && !e.Removido);
//            if (!empresaExists)
//            {
//                throw new InvalidOperationException($"Empresa con ID {empresaId} no encontrada");
//            }

//            var menus = await _context.Menus
//                .Include(m => m.Tipo)
//                .Where(m => m.IdEmpresa == empresaId && !m.Removido && m.IdPadre == null)
//                .OrderBy(m => m.Orden)
//                .ToListAsync();

//            var result = new List<MenuTreeDTO>();
//            foreach (var menu in menus)
//            {
//                var menuTree = await BuildMenuTreeAsync(menu);
//                result.Add(menuTree);
//            }

//            return result;
//        }

//        public async Task<IEnumerable<MenuUsuarioDTO>> GetMenusByUsuarioIdAsync(int usuarioId, int empresaId)
//        {
//            // Verificar que el usuario existe
//            var usuarioExists = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId && !u.Removido);
//            if (!usuarioExists)
//            {
//                throw new InvalidOperationException($"Usuario con ID {usuarioId} no encontrado");
//            }

//            // Verificar que la empresa existe
//            var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == empresaId && !e.Removido);
//            if (!empresaExists)
//            {
//                throw new InvalidOperationException($"Empresa con ID {empresaId} no encontrada");
//            }

//            // Obtener los permisos del usuario
//            var permisosIds = await _context.UsuarioRoles
//                .Where(ur => ur.IdUsuario == usuarioId)
//                .Join(_context.RolPermisos,
//                    ur => ur.IdRol,
//                    rp => rp.IdRol,
//                    (ur, rp) => rp.IdPermiso)
//                .Distinct()
//                .ToListAsync();

//            // Obtener menús principales (sin padre)
//            var menus = await _context.Menus
//                .Include(m => m.Tipo)
//                .Where(m => m.IdEmpresa == empresaId && !m.Removido && m.IdPadre == null &&
//                            (!m.VerificaPermiso || !m.IdPermiso.HasValue || permisosIds.Contains(m.IdPermiso.Value)))
//                .OrderBy(m => m.Orden)
//                .ToListAsync();

//            var result = new List<MenuUsuarioDTO>();
//            foreach (var menu in menus)
//            {
//                var menuUsuario = await BuildMenuUsuarioAsync(menu, permisosIds);
//                if (menuUsuario != null)
//                {
//                    result.Add(menuUsuario);
//                }
//            }

//            return result;
//        }

//        #region Métodos privados

//        private MenuDTO MapToMenuDTO(Menu menu)
//        {
//            return new MenuDTO
//            {
//                Id = menu.Id,
//                Nombre = menu.Nombre,
//                Nemonico = menu.Nemonico,
//                Descripcion = menu.Descripcion,
//                VerificaPermiso = menu.VerificaPermiso,
//                Orden = menu.Orden,
//                IdPadre = menu.IdPadre,
//                Url = menu.Url,
//                IdTipo = menu.IdTipo,
//                TipoNombre = menu.Tipo?.Nombre,
//                IdEmpresa = menu.IdEmpresa,
//                EmpresaNombre = menu.Empresa?.Nombre,
//                IdPermiso = menu.VerificaPermiso ? menu.IdPermiso : null,
//                PermisoNombre = menu.VerificaPermiso && menu.IdPermiso.HasValue ? menu.Permiso?.Nemonico : null
//            };
//        }

//        private async Task<MenuTreeDTO> BuildMenuTreeAsync(Menu menu)
//        {
//            var menuTree = new MenuTreeDTO
//            {
//                Id = menu.Id,
//                Nombre = menu.Nombre,
//                Nemonico = menu.Nemonico,
//                Url = menu.Url,
//                Orden = menu.Orden,
//                Tipo = menu.Tipo?.Nombre
//            };

//            // Obtener submenús recursivamente
//            var submenus = await _context.Menus
//                .Include(m => m.Tipo)
//                .Where(m => m.IdPadre == menu.Id && !m.Removido)
//                .OrderBy(m => m.Orden)
//                .ToListAsync();

//            foreach (var submenu in submenus)
//            {
//                var submenuTree = await BuildMenuTreeAsync(submenu);
//                menuTree.SubMenus.Add(submenuTree);
//            }

//            return menuTree;
//        }

//        private async Task<MenuUsuarioDTO> BuildMenuUsuarioAsync(Menu menu, List<int> permisosIds)
//        {
//            // Si el menú requiere permiso, verificar que el usuario lo tenga
//            if (menu.VerificaPermiso && menu.IdPermiso.HasValue && !permisosIds.Contains(menu.IdPermiso.Value))
//            {
//                return null; // El usuario no tiene permiso para este menú
//            }

//            var menuUsuario = new MenuUsuarioDTO
//            {
//                Id = menu.Id,
//                Nombre = menu.Nombre,
//                Nemonico = menu.Nemonico,
//                Url = menu.Url,
//                Orden = menu.Orden,
//                Tipo = menu.Tipo?.Nombre
//            };

//            // Obtener submenús recursivamente
//            var submenus = await _context.Menus
//                .Include(m => m.Tipo)
//                .Where(m => m.IdPadre == menu.Id && !m.Removido)
//                .OrderBy(m => m.Orden)
//                .ToListAsync();

//            foreach (var submenu in submenus)
//            {
//                var submenuUsuario = await BuildMenuUsuarioAsync(submenu, permisosIds);
//                if (submenuUsuario != null) // Solo añadir si el usuario tiene permiso
//                {
//                    menuUsuario.SubMenus.Add(submenuUsuario);
//                }
//            }

//            // Si es un menú de tipo carpeta y no tiene submenús después de filtrar permisos, no mostrarlo
//            if ((menu.Tipo?.Nombre == "Carpeta" || menu.Tipo?.Nemonico == "CARPETA") && !menuUsuario.SubMenus.Any())
//            {
//                return null;
//            }

//            return menuUsuario;
//        }

//        #endregion
//    }
//}