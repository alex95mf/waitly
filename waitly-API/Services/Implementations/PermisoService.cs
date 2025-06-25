using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using waitly_API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace waitly_API.Services.Implementations
{
    public class PermisoService : IPermisoService
    {
        private readonly ApplicationDbContext _context;

        public PermisoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PermisoDTO>> GetAllPermisosAsync()
        {
            var permisos = await _context.Permisos
                .Where(p => !p.Removido)
                .Include(p => p.Tipo)
                .Include(p => p.PermisosPantallas)
                    .ThenInclude(pp => pp.Pantalla)
                .ToListAsync();

            return permisos.Select(p => MapPermisoToDTO(p));
        }

        public async Task<PermisoDTO> GetPermisoByIdAsync(int id)
        {
            var permiso = await _context.Permisos
                .Where(p => p.Id == id && !p.Removido)
                .Include(p => p.Tipo)
                .Include(p => p.PermisosPantallas)
                    .ThenInclude(pp => pp.Pantalla)
                .FirstOrDefaultAsync();

            if (permiso == null)
                return null;

            return MapPermisoToDTO(permiso);
        }

        public async Task<PermisoDTO> CreatePermisoAsync(CreatePermisoDTO createPermisoDto)
        {
            // Verificar si el tipo existe
            var tipoExists = await _context.Catalogos.AnyAsync(c => c.Id == createPermisoDto.IdTipo && !c.Removido);
            if (!tipoExists)
            {
                throw new InvalidOperationException($"Tipo de permiso con ID {createPermisoDto.IdTipo} no encontrado");
            }

            // Verificar si ya existe un permiso con el mismo nombre
            if (await _context.Permisos.AnyAsync(p => p.Nemonico == createPermisoDto.Nemonico && !p.Removido))
            {
                throw new InvalidOperationException($"Ya existe un permiso con el nombre '{createPermisoDto.Nemonico}'");
            }

            // Crear nueva entidad Permiso
            var permiso = new Permiso
            {
                Nemonico = createPermisoDto.Nemonico,
                Descripcion = createPermisoDto.Descripcion,
                IdTipo = createPermisoDto.IdTipo,
                FechaCreacion = DateTime.Now,
                IpCreacion = "IP.PRUEBA",
                Removido = false,
                PermisosPantallas = new List<PermisoPantalla>()
            };

            // Asignar pantallas si se proporcionaron
            if (createPermisoDto.IdPantallas != null && createPermisoDto.IdPantallas.Count > 0)
            {
                // Verificar que las pantallas existan
                var pantallasExistentes = await _context.Pantallas
                    .Where(p => createPermisoDto.IdPantallas.Contains(p.Id) && !p.Removido)
                    .Select(p => p.Id)
                    .ToListAsync();

                if (pantallasExistentes.Count != createPermisoDto.IdPantallas.Count)
                {
                    var noEncontradas = createPermisoDto.IdPantallas.Except(pantallasExistentes).ToList();
                    throw new InvalidOperationException($"Las siguientes pantallas no fueron encontradas: {string.Join(", ", noEncontradas)}");
                }

                // Asignar pantallas al permiso
                foreach (var pantallaId in createPermisoDto.IdPantallas)
                {
                    permiso.PermisosPantallas.Add(new PermisoPantalla
                    {
                        IdPermiso = permiso.Id,
                        IdPantalla = pantallaId
                    });
                }
            }

            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();

            // Obtener el permiso completo con sus relaciones
            return await GetPermisoByIdAsync(permiso.Id);
        }

        public async Task<bool> UpdatePermisoAsync(int id, UpdatePermisoDTO updatePermisoDto)
        {
            var permiso = await _context.Permisos
                .Include(p => p.PermisosPantallas)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Removido);

            if (permiso == null)
            {
                return false;
            }

            // Verificar si ya existe un permiso con el mismo nombre (si se está actualizando)
            if (!string.IsNullOrEmpty(updatePermisoDto.Nemonico) &&
                updatePermisoDto.Nemonico != permiso.Nemonico &&
                await _context.Permisos.AnyAsync(p => p.Nemonico == updatePermisoDto.Nemonico && !p.Removido))
            {
                throw new InvalidOperationException($"Ya existe un permiso con el nombre '{updatePermisoDto.Nemonico}'");
            }

            // Actualizar campos proporcionados
            if (!string.IsNullOrEmpty(updatePermisoDto.Nemonico))
                permiso.Nemonico = updatePermisoDto.Nemonico;

            if (!string.IsNullOrEmpty(updatePermisoDto.Descripcion))
                permiso.Descripcion = updatePermisoDto.Descripcion;

            if (updatePermisoDto.IdTipo.HasValue)
            {
                // Verificar si el tipo existe
                var tipoExists = await _context.Catalogos.AnyAsync(c => c.Id == updatePermisoDto.IdTipo.Value && !c.Removido);
                if (!tipoExists)
                {
                    throw new InvalidOperationException($"Tipo de permiso con ID {updatePermisoDto.IdTipo.Value} no encontrado");
                }

                permiso.IdTipo = updatePermisoDto.IdTipo.Value;
            }

            // Actualizar pantallas si se proporcionaron
            if (updatePermisoDto.IdPantallas != null)
            {
                // Eliminar pantallas existentes
                _context.PermisosPantallas.RemoveRange(permiso.PermisosPantallas);

                // Verificar que las nuevas pantallas existan
                var pantallasExistentes = await _context.Pantallas
                    .Where(p => updatePermisoDto.IdPantallas.Contains(p.Id) && !p.Removido)
                    .Select(p => p.Id)
                    .ToListAsync();

                if (pantallasExistentes.Count != updatePermisoDto.IdPantallas.Count)
                {
                    var noEncontradas = updatePermisoDto.IdPantallas.Except(pantallasExistentes).ToList();
                    throw new InvalidOperationException($"Las siguientes pantallas no fueron encontradas: {string.Join(", ", noEncontradas)}");
                }

                // Asignar nuevas pantallas
                foreach (var pantallaId in updatePermisoDto.IdPantallas)
                {
                    permiso.PermisosPantallas.Add(new PermisoPantalla
                    {
                        IdPermiso = permiso.Id,
                        IdPantalla = pantallaId
                    });
                }
            }

            permiso.FechaModificacion = DateTime.Now;
            permiso.IpModificacion = "IP.PRUEBA";

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PermisoExistsAsync(id))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeletePermisoAsync(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);

            if (permiso == null)
            {
                return false;
            }

            // Verificar si el permiso está en uso por algún rol
            var permisoEnUso = await _context.RolPermisos.AnyAsync(rp => rp.IdPermiso == id);
            if (permisoEnUso)
            {
                throw new InvalidOperationException($"No se puede eliminar el permiso porque está asignado a uno o más roles");
            }

            // Borrado lógico
            permiso.Removido = true;
            permiso.FechaEliminacion = DateTime.Now;
            permiso.IpEliminacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PermisoExistsAsync(int id)
        {
            return await _context.Permisos.AnyAsync(p => p.Id == id && !p.Removido);
        }

        public async Task<IEnumerable<PermisoDTO>> GetPermisosByUsuarioIdAsync(int usuarioId)
        {
            // Verificar si el usuario existe
            var usuarioExists = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId && !u.Removido);
            if (!usuarioExists)
            {
                throw new InvalidOperationException($"Usuario con ID {usuarioId} no encontrado");
            }

            // Obtener todos los roles del usuario
            var roleIds = await _context.UsuarioRoles
                .Where(ur => ur.IdUsuario == usuarioId)
                .Select(ur => ur.IdRol)
                .ToListAsync();

            // Obtener todos los permisos asociados a esos roles
            var permisos = await _context.RolPermisos
                .Where(rp => roleIds.Contains(rp.IdRol))
                .Include(rp => rp.Permiso)
                    .ThenInclude(p => p.Tipo)
                .Include(rp => rp.Permiso)
                    .ThenInclude(p => p.PermisosPantallas)
                        .ThenInclude(pp => pp.Pantalla)
                .Where(rp => !rp.Permiso.Removido)
                .Select(rp => rp.Permiso)
                .Distinct()
                .ToListAsync();

            return permisos.Select(p => MapPermisoToDTO(p));
        }

        // Método auxiliar para mapear un permiso a su DTO
        private PermisoDTO MapPermisoToDTO(Permiso permiso)
        {
            return new PermisoDTO
            {
                Id = permiso.Id,
                Nemonico = permiso.Nemonico,
                Descripcion = permiso.Descripcion,
                IdTipo = permiso.IdTipo,
                NombreTipo = permiso.Tipo?.Nombre ?? "Tipo Desconocido",
                Pantallas = permiso.PermisosPantallas?
                    .Where(pp => pp.Pantalla != null && !pp.Pantalla.Removido)
                    .Select(pp => pp.Pantalla.Nombre)
                    .ToList() ?? new List<string>()
            };
        }
    }
}