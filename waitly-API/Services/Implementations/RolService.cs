// Services/Implementations/RolService.cs
using iSit_API.Models.DTOs;
using iSit_API.Models.Entities;
using iSit_API.Services.Interfaces;
using ISit_API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSit_API.Services.Implementations
{
    public class RolService : IRolService
    {
        private readonly ApplicationDbContext _context;

        public RolService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolDTO>> GetAllRolesAsync()
        {
            var roles = await _context.Roles
                .Where(r => !r.Removido)
                .Include(r => r.Empresa)
                .Include(r => r.RolPermisos)
                    .ThenInclude(rp => rp.Permiso)
                .ToListAsync();

            return roles.Select(r => MapRolToDTO(r));
        }

        public async Task<RolDTO> GetRolByIdAsync(int id)
        {
            var rol = await _context.Roles
                .Where(r => r.Id == id && !r.Removido)
                .Include(r => r.Empresa)
                .Include(r => r.RolPermisos)
                    .ThenInclude(rp => rp.Permiso)
                .FirstOrDefaultAsync();

            if (rol == null)
                return null;

            return MapRolToDTO(rol);
        }

        public async Task<RolDTO> CreateRolAsync(CreateRolDTO createRolDto)
        {
            // Verificar si la empresa existe
            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(e => e.Id == createRolDto.IdEmpresa && !e.Removido);

            if (empresa == null)
            {
                throw new InvalidOperationException($"Empresa con ID {createRolDto.IdEmpresa} no encontrada");
            }

            // Verificar si ya existe un rol con el mismo nombre en la misma empresa
            if (await _context.Roles.AnyAsync(r => r.Nombre == createRolDto.Nombre &&
                                                r.IdEmpresa == createRolDto.IdEmpresa &&
                                                !r.Removido))
            {
                throw new InvalidOperationException($"Ya existe un rol con el nombre '{createRolDto.Nombre}' en esta empresa");
            }

            // Crear nueva entidad Rol
            var rol = new Rol
            {
                Nombre = createRolDto.Nombre,
                Descripcion = createRolDto.Descripcion,
                IdEmpresa = createRolDto.IdEmpresa,
                FechaCreacion = DateTime.Now,
                IpCreacion = "IP.PRUEBA",
                Removido = false,
                RolPermisos = new List<RolPermiso>()
            };

            // Asignar permisos si se proporcionaron
            if (createRolDto.IdPermisos != null && createRolDto.IdPermisos.Count > 0)
            {
                // Verificar que los permisos existan
                var permisosExistentes = await _context.Permisos
                    .Where(p => createRolDto.IdPermisos.Contains(p.Id) && !p.Removido)
                    .Select(p => p.Id)
                    .ToListAsync();

                if (permisosExistentes.Count != createRolDto.IdPermisos.Count)
                {
                    var noEncontrados = createRolDto.IdPermisos.Except(permisosExistentes).ToList();
                    throw new InvalidOperationException($"Los siguientes permisos no fueron encontrados: {string.Join(", ", noEncontrados)}");
                }

                // Asignar permisos al rol
                foreach (var permisoId in createRolDto.IdPermisos)
                {
                    rol.RolPermisos.Add(new RolPermiso
                    {
                        IdRol = rol.Id,
                        IdPermiso = permisoId
                    });
                }
            }

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            // Obtener el rol completo con sus relaciones
            return await GetRolByIdAsync(rol.Id);
        }

        public async Task<bool> UpdateRolAsync(int id, UpdateRolDTO updateRolDto)
        {
            var rol = await _context.Roles
                .Include(r => r.RolPermisos)
                .FirstOrDefaultAsync(r => r.Id == id && !r.Removido);

            if (rol == null)
            {
                return false;
            }

            // Verificar si ya existe un rol con el mismo nombre en la misma empresa (si se está actualizando)
            if (!string.IsNullOrEmpty(updateRolDto.Nombre) &&
                updateRolDto.Nombre != rol.Nombre &&
                await _context.Roles.AnyAsync(r => r.Nombre == updateRolDto.Nombre &&
                                                r.IdEmpresa == (updateRolDto.IdEmpresa ?? rol.IdEmpresa) &&
                                                !r.Removido))
            {
                throw new InvalidOperationException($"Ya existe un rol con el nombre '{updateRolDto.Nombre}' en esta empresa");
            }

            // Actualizar campos proporcionados
            if (!string.IsNullOrEmpty(updateRolDto.Nombre))
                rol.Nombre = updateRolDto.Nombre;

            if (!string.IsNullOrEmpty(updateRolDto.Descripcion))
                rol.Descripcion = updateRolDto.Descripcion;

            if (updateRolDto.IdEmpresa.HasValue)
            {
                // Verificar si la empresa existe
                var empresaExists = await _context.Empresas
                    .AnyAsync(e => e.Id == updateRolDto.IdEmpresa.Value && !e.Removido);

                if (!empresaExists)
                {
                    throw new InvalidOperationException($"Empresa con ID {updateRolDto.IdEmpresa.Value} no encontrada");
                }

                rol.IdEmpresa = updateRolDto.IdEmpresa.Value;
            }

            // Actualizar permisos si se proporcionaron
            if (updateRolDto.IdPermisos != null)
            {
                // Eliminar permisos existentes
                _context.RolPermisos.RemoveRange(rol.RolPermisos);

                // Verificar que los nuevos permisos existan
                var permisosExistentes = await _context.Permisos
                    .Where(p => updateRolDto.IdPermisos.Contains(p.Id) && !p.Removido)
                    .Select(p => p.Id)
                    .ToListAsync();

                if (permisosExistentes.Count != updateRolDto.IdPermisos.Count)
                {
                    var noEncontrados = updateRolDto.IdPermisos.Except(permisosExistentes).ToList();
                    throw new InvalidOperationException($"Los siguientes permisos no fueron encontrados: {string.Join(", ", noEncontrados)}");
                }

                // Asignar nuevos permisos
                foreach (var permisoId in updateRolDto.IdPermisos)
                {
                    rol.RolPermisos.Add(new RolPermiso
                    {
                        IdRol = rol.Id,
                        IdPermiso = permisoId
                    });
                }
            }

            rol.FechaModificacion = DateTime.Now;
            rol.IpModificacion = "IP.PRUEBA";

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RolExistsAsync(id))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteRolAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);

            if (rol == null)
            {
                return false;
            }

            // Verificar si el rol está en uso por algún usuario
            var rolEnUso = await _context.UsuarioRoles.AnyAsync(ur => ur.IdRol == id);
            if (rolEnUso)
            {
                throw new InvalidOperationException($"No se puede eliminar el rol porque está asignado a uno o más usuarios");
            }

            // Borrado lógico
            rol.Removido = true;
            rol.FechaEliminacion = DateTime.Now;
            rol.IpEliminacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RolExistsAsync(int id)
        {
            return await _context.Roles.AnyAsync(r => r.Id == id && !r.Removido);
        }

        public async Task<IEnumerable<RolDTO>> GetRolesByEmpresaIdAsync(int empresaId)
        {
            // Verificar si la empresa existe
            var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == empresaId && !e.Removido);
            if (!empresaExists)
            {
                throw new InvalidOperationException($"Empresa con ID {empresaId} no encontrada");
            }

            var roles = await _context.Roles
                .Where(r => r.IdEmpresa == empresaId && !r.Removido)
                .Include(r => r.Empresa)
                .Include(r => r.RolPermisos)
                    .ThenInclude(rp => rp.Permiso)
                .ToListAsync();

            return roles.Select(r => MapRolToDTO(r));
        }

        // Método auxiliar para mapear un rol a su DTO
        private RolDTO MapRolToDTO(Rol rol)
        {
            return new RolDTO
            {
                Id = rol.Id,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion,
                IdEmpresa = rol.IdEmpresa,
                NombreEmpresa = rol.Empresa?.Nombre ?? "Empresa Desconocida",
                Permisos = rol.RolPermisos?
                    .Where(rp => rp.Permiso != null && !rp.Permiso.Removido)
                    .Select(rp => rp.Permiso.Nemonico)
                    .ToList() ?? new List<string>()
            };
        }
    }
}