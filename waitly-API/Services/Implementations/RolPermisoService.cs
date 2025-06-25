using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using waitly_API.Data;
using Microsoft.EntityFrameworkCore;

namespace waitly_API.Services.Implementations
{
    public class RolPermisoService : IRolPermisoService
    {
        private readonly ApplicationDbContext _context;

        public RolPermisoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PermisoDTO>> GetPermisosByRolIdAsync(int rolId)
        {
            // Verificar si el rol existe
            var rolExists = await _context.Roles.AnyAsync(r => r.Id == rolId && !r.Removido);
            if (!rolExists)
            {
                throw new InvalidOperationException($"Rol con ID {rolId} no encontrado");
            }

            var permisos = await _context.RolPermisos
                .Where(rp => rp.IdRol == rolId)
                .Include(rp => rp.Permiso)
                    .ThenInclude(p => p.Tipo)
                .Include(rp => rp.Permiso)
                    .ThenInclude(p => p.PermisosPantallas)
                        .ThenInclude(pp => pp.Pantalla)
                .Where(rp => !rp.Permiso.Removido)
                .Select(rp => rp.Permiso)
                .ToListAsync();

            return permisos.Select(p => new PermisoDTO
            {
                Id = p.Id,
                Nemonico = p.Nemonico,
                Descripcion = p.Descripcion,
                IdTipo = p.IdTipo,
                NombreTipo = p.Tipo?.Nombre ?? "Tipo Desconocido",
                Pantallas = p.PermisosPantallas?
                    .Where(pp => pp.Pantalla != null && !pp.Pantalla.Removido)
                    .Select(pp => pp.Pantalla.Nombre)
                    .ToList() ?? new List<string>()
            });
        }

        public async Task<IEnumerable<RolDTO>> GetRolesByPermisoIdAsync(int permisoId)
        {
            // Verificar si el permiso existe
            var permisoExists = await _context.Permisos.AnyAsync(p => p.Id == permisoId && !p.Removido);
            if (!permisoExists)
            {
                throw new InvalidOperationException($"Permiso con ID {permisoId} no encontrado");
            }

            var roles = await _context.RolPermisos
                .Where(rp => rp.IdPermiso == permisoId)
                .Include(rp => rp.Rol)
                    .ThenInclude(r => r.Empresa)
                .Where(rp => !rp.Rol.Removido)
                .Select(rp => rp.Rol)
                .ToListAsync();

            return roles.Select(r => new RolDTO
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Descripcion = r.Descripcion,
                IdEmpresa = r.IdEmpresa,
                NombreEmpresa = r.Empresa?.Nombre ?? "Empresa Desconocida",
                Permisos = new List<string>() // No incluimos permisos aquí para evitar recursión
            });
        }

        public async Task AsignarPermisosARolAsync(int rolId, List<int> permisoIds)
        {
            // Verificar si el rol existe
            var rol = await _context.Roles
                .Include(r => r.RolPermisos)
                .FirstOrDefaultAsync(r => r.Id == rolId && !r.Removido);

            if (rol == null)
            {
                throw new InvalidOperationException($"Rol con ID {rolId} no encontrado");
            }

            // Verificar si todos los permisos existen
            var permisosExistentes = await _context.Permisos
                .Where(p => permisoIds.Contains(p.Id) && !p.Removido)
                .Select(p => p.Id)
                .ToListAsync();

            if (permisosExistentes.Count != permisoIds.Count)
            {
                var noEncontrados = permisoIds.Except(permisosExistentes).ToList();
                throw new InvalidOperationException($"Los siguientes permisos no fueron encontrados: {string.Join(", ", noEncontrados)}");
            }

            // Obtener los permisos que ya están asignados al rol
            var permisosActuales = rol.RolPermisos
                .Select(rp => rp.IdPermiso)
                .ToList();

            // Filtrar solo los permisos que no están ya asignados
            var nuevosPermisos = permisoIds
                .Except(permisosActuales)
                .ToList();

            // Crear nuevas relaciones
            foreach (var permisoId in nuevosPermisos)
            {
                rol.RolPermisos.Add(new RolPermiso
                {
                    IdRol = rolId,
                    IdPermiso = permisoId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task QuitarPermisosDeRolAsync(int rolId, List<int> permisoIds)
        {
            // Verificar si el rol existe
            var rol = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == rolId && !r.Removido);

            if (rol == null)
            {
                throw new InvalidOperationException($"Rol con ID {rolId} no encontrado");
            }

            // Buscar las relaciones existentes
            var relacionesAEliminar = await _context.RolPermisos
                .Where(rp => rp.IdRol == rolId && permisoIds.Contains(rp.IdPermiso))
                .ToListAsync();

            if (relacionesAEliminar.Count == 0)
            {
                // No hay relaciones para eliminar
                return;
            }

            // Eliminar las relaciones
            _context.RolPermisos.RemoveRange(relacionesAEliminar);
            await _context.SaveChangesAsync();
        }
    }
}