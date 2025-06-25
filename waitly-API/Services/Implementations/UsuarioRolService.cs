using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using waitly_API.Data;
using Microsoft.EntityFrameworkCore;

namespace waitly_API.Services.Implementations
{
    public class UsuarioRolService : IUsuarioRolService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRolService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolDTO>> GetRolesByUsuarioIdAsync(int usuarioId)
        {
            // Verificar que el usuario existe
            var usuarioExists = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId && !u.Removido);
            if (!usuarioExists)
            {
                throw new InvalidOperationException($"Usuario con ID {usuarioId} no encontrado");
            }

            var roles = await _context.UsuarioRoles
                .Where(ur => ur.IdUsuario == usuarioId)
                .Include(ur => ur.Rol)
                .Select(ur => new RolDTO
                {
                    Id = ur.Rol.Id,
                    Nombre = ur.Rol.Nombre,
                    Descripcion = ur.Rol.Descripcion,
                    IdEmpresa = ur.Rol.IdEmpresa
                })
                .ToListAsync();

            return roles;
        }

        public async Task<IEnumerable<UsuarioDTO>> GetUsuariosByRolIdAsync(int rolId)
        {
            // Verificar que el rol existe
            var rolExists = await _context.Roles.AnyAsync(r => r.Id == rolId && !r.Removido);
            if (!rolExists)
            {
                throw new InvalidOperationException($"Rol con ID {rolId} no encontrado");
            }

            var usuarios = await _context.UsuarioRoles
                .Where(ur => ur.IdRol == rolId)
                .Include(ur => ur.Usuario)
                .Where(ur => !ur.Usuario.Removido)
                .Select(ur => new UsuarioDTO
                {
                    Id = ur.Usuario.Id,
                    User = ur.Usuario.User,
                    Nombres = ur.Usuario.Nombres,
                    Apellidos = ur.Usuario.Apellidos,
                    Roles = _context.UsuarioRoles
                        .Where(r => r.IdUsuario == ur.Usuario.Id)
                        .Include(r => r.Rol)
                        .Select(r => r.Rol.Nombre)
                        .ToList(),
                    Empresas = _context.UsuarioEmpresas
                        .Where(ue => ue.IdUsuario == ur.Usuario.Id)
                        .Include(ue => ue.Empresa)
                        .Select(ue => new EmpresaDTO
                        {
                            Id = ue.Empresa.Id,
                            Nombre = ue.Empresa.Nombre,
                            Nemonico = ue.Empresa.Nemonico
                        })
                        .ToList()
                })
                .ToListAsync();

            return usuarios;
        }

        public async Task AsignarRolesAUsuarioAsync(int usuarioId, List<int> rolIds)
        {
            // Verificar que el usuario existe
            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                .FirstOrDefaultAsync(u => u.Id == usuarioId && !u.Removido);

            if (usuario == null)
            {
                throw new InvalidOperationException($"Usuario con ID {usuarioId} no encontrado");
            }

            // Verificar que los roles existen
            var roles = await _context.Roles
                .Where(r => rolIds.Contains(r.Id) && !r.Removido)
                .ToListAsync();

            if (roles.Count != rolIds.Count)
            {
                throw new InvalidOperationException("Uno o más roles no existen");
            }

            // Obtener los roles que ya tiene el usuario
            var rolesActuales = usuario.UsuarioRoles.Select(ur => ur.IdRol).ToList();

            // Crear las nuevas relaciones usuario-rol
            foreach (var rolId in rolIds)
            {
                // Verificar que el rol no esté asignado ya al usuario
                if (!rolesActuales.Contains(rolId))
                {
                    usuario.UsuarioRoles.Add(new UsuarioRol
                    {
                        IdUsuario = usuarioId,
                        IdRol = rolId
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task QuitarRolesDeUsuarioAsync(int usuarioId, List<int> rolIds)
        {
            // Verificar que el usuario existe
            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                .FirstOrDefaultAsync(u => u.Id == usuarioId && !u.Removido);

            if (usuario == null)
            {
                throw new InvalidOperationException($"Usuario con ID {usuarioId} no encontrado");
            }

            // Obtener las relaciones usuario-rol a eliminar
            var rolesAEliminar = usuario.UsuarioRoles
                .Where(ur => rolIds.Contains(ur.IdRol))
                .ToList();

            // Eliminar las relaciones
            foreach (var rolUsuario in rolesAEliminar)
            {
                _context.UsuarioRoles.Remove(rolUsuario);
            }

            await _context.SaveChangesAsync();
        }
    }
}