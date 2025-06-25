using ISit_API.Data;
using iSit_API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using iSit_API.Services.Interfaces;
using iSit_API.Models.Entities;

namespace iSit_API.Services.Implementations
{
    public class UsuarioEmpresaService : IUsuarioEmpresaService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioEmpresaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmpresaDTO>> GetEmpresasByUsuarioIdAsync(int usuarioId)
        {
            // Verificar si el usuario existe
            var usuarioExists = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId && !u.Removido);
            if (!usuarioExists)
            {
                throw new InvalidOperationException($"Usuario con ID {usuarioId} no encontrado");
            }

            var empresas = await _context.UsuarioEmpresas
                .Where(ue => ue.IdUsuario == usuarioId)
                .Include(ue => ue.Empresa)
                .Where(ue => !ue.Empresa.Removido)
                .Select(ue => new EmpresaDTO
                {
                    Id = ue.Empresa.Id,
                    Nombre = ue.Empresa.Nombre,
                    Nemonico = ue.Empresa.Nemonico
                })
                .ToListAsync();

            return empresas;
        }

        public async Task<IEnumerable<UsuarioDTO>> GetUsuariosByEmpresaIdAsync(int empresaId)
        {
            // Verificar si la empresa existe
            var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == empresaId && !e.Removido);
            if (!empresaExists)
            {
                throw new InvalidOperationException($"Empresa con ID {empresaId} no encontrada");
            }

            var usuarios = await _context.UsuarioEmpresas
                .Where(ue => ue.IdEmpresa == empresaId)
                .Include(ue => ue.Usuario)
                .Where(ue => !ue.Usuario.Removido)
                .Select(ue => new UsuarioDTO
                {
                    Id = ue.Usuario.Id,
                    User = ue.Usuario.User,
                    Nombres = ue.Usuario.Nombres,
                    Apellidos = ue.Usuario.Apellidos
                    // No incluimos los roles y empresas anidados para evitar recursión
                })
                .ToListAsync();

            return usuarios;
        }

        public async Task AsignarEmpresasAUsuarioAsync(int usuarioId, List<int> empresaIds)
        {
            // Verificar si el usuario existe
            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioEmpresas)
                .FirstOrDefaultAsync(u => u.Id == usuarioId && !u.Removido);

            if (usuario == null)
            {
                throw new InvalidOperationException($"Usuario con ID {usuarioId} no encontrado");
            }

            // Verificar si todas las empresas existen
            var empresasExistentes = await _context.Empresas
                .Where(e => empresaIds.Contains(e.Id) && !e.Removido)
                .Select(e => e.Id)
                .ToListAsync();

            if (empresasExistentes.Count != empresaIds.Count)
            {
                var noEncontradas = empresaIds.Except(empresasExistentes).ToList();
                throw new InvalidOperationException($"Las siguientes empresas no fueron encontradas: {string.Join(", ", noEncontradas)}");
            }

            // Obtener las empresas que ya están asignadas al usuario
            var empresasActuales = usuario.UsuarioEmpresas
                .Select(ue => ue.IdEmpresa)
                .ToList();

            // Filtrar solo las empresas que no están ya asignadas
            var nuevasEmpresas = empresaIds
                .Except(empresasActuales)
                .ToList();

            // Crear nuevas relaciones
            foreach (var empresaId in nuevasEmpresas)
            {
                usuario.UsuarioEmpresas.Add(new UsuarioEmpresa
                {
                    IdUsuario = usuarioId,
                    IdEmpresa = empresaId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task QuitarEmpresasDeUsuarioAsync(int usuarioId, List<int> empresaIds)
        {
            // Verificar si el usuario existe
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == usuarioId && !u.Removido);

            if (usuario == null)
            {
                throw new InvalidOperationException($"Usuario con ID {usuarioId} no encontrado");
            }

            // Buscar las relaciones existentes
            var relacionesAEliminar = await _context.UsuarioEmpresas
                .Where(ue => ue.IdUsuario == usuarioId && empresaIds.Contains(ue.IdEmpresa))
                .ToListAsync();

            if (relacionesAEliminar.Count == 0)
            {
                // No hay relaciones para eliminar
                return;
            }

            // Eliminar las relaciones
            _context.UsuarioEmpresas.RemoveRange(relacionesAEliminar);
            await _context.SaveChangesAsync();
        }
    }
}
