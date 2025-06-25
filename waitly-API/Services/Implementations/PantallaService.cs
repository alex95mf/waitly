using waitly_API.Data;
using Microsoft.EntityFrameworkCore;
using waitly_API.Models.DTOs;
using waitly_API.Services.Interfaces;
using waitly_API.Models.Entities;

namespace waitly_API.Services.Implementations
{
    public class PantallaService : IPantallaService
    {
        private readonly ApplicationDbContext _context;

        public PantallaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PantallaDTO>> GetAllPantallasAsync()
        {
            var pantallas = await _context.Pantallas
                .Where(p => !p.Removido)
                .Include(p => p.Menu)
                .ToListAsync();

            return pantallas.Select(p => MapPantallaToDTO(p));
        }

        public async Task<PantallaDTO> GetPantallaByIdAsync(int id)
        {
            var pantalla = await _context.Pantallas
                .Where(p => p.Id == id && !p.Removido)
                .Include(p => p.Menu)
                .FirstOrDefaultAsync();

            if (pantalla == null)
                return null;

            return MapPantallaToDTO(pantalla);
        }

        public async Task<PantallaDTO> CreatePantallaAsync(CreatePantallaDTO createPantallaDto)
        {
            // Verificar si el menú existe
            if (!await _context.Menus.AnyAsync(m => m.Id == createPantallaDto.IdMenu && !m.Removido))
            {
                throw new InvalidOperationException($"El menú con ID {createPantallaDto.IdMenu} no existe");
            }

            // Verificar si ya existe una pantalla con el mismo nemónico
            if (await _context.Pantallas.AnyAsync(p => p.Nemonico == createPantallaDto.Nemonico && !p.Removido))
            {
                throw new InvalidOperationException($"Ya existe una pantalla con el nemónico '{createPantallaDto.Nemonico}'");
            }

            // Crear nueva entidad Pantalla
            var pantalla = new Pantalla
            {
                Nombre = createPantallaDto.Nombre,
                Nemonico = createPantallaDto.Nemonico,
                Descripcion = createPantallaDto.Descripcion,
                VerificaPermiso = createPantallaDto.VerificaPermiso,
                IdMenu = createPantallaDto.IdMenu,
                FechaCreacion = DateTime.Now,
                IpCreacion = "IP.PRUEBA",
                Removido = false
            };

            _context.Pantallas.Add(pantalla);
            await _context.SaveChangesAsync();

            // Obtener la pantalla completa con sus relaciones
            return await GetPantallaByIdAsync(pantalla.Id);
        }

        public async Task<bool> UpdatePantallaAsync(int id, UpdatePantallaDTO updatePantallaDto)
        {
            var pantalla = await _context.Pantallas.FindAsync(id);

            if (pantalla == null || pantalla.Removido)
            {
                return false;
            }

            // Verificar si el nemónico ya existe (si se está actualizando)
            if (!string.IsNullOrEmpty(updatePantallaDto.Nemonico) &&
                updatePantallaDto.Nemonico != pantalla.Nemonico &&
                await _context.Pantallas.AnyAsync(p => p.Nemonico == updatePantallaDto.Nemonico && !p.Removido))
            {
                throw new InvalidOperationException($"Ya existe una pantalla con el nemónico '{updatePantallaDto.Nemonico}'");
            }

            // Verificar si el menú existe (si se está actualizando)
            if (updatePantallaDto.IdMenu.HasValue &&
                !await _context.Menus.AnyAsync(m => m.Id == updatePantallaDto.IdMenu.Value && !m.Removido))
            {
                throw new InvalidOperationException($"El menú con ID {updatePantallaDto.IdMenu.Value} no existe");
            }

            // Actualizar solo los campos proporcionados
            if (!string.IsNullOrEmpty(updatePantallaDto.Nombre))
                pantalla.Nombre = updatePantallaDto.Nombre;

            if (!string.IsNullOrEmpty(updatePantallaDto.Nemonico))
                pantalla.Nemonico = updatePantallaDto.Nemonico;

            if (!string.IsNullOrEmpty(updatePantallaDto.Descripcion))
                pantalla.Descripcion = updatePantallaDto.Descripcion;

            if (updatePantallaDto.VerificaPermiso.HasValue)
                pantalla.VerificaPermiso = updatePantallaDto.VerificaPermiso.Value;

            if (updatePantallaDto.IdMenu.HasValue)
                pantalla.IdMenu = updatePantallaDto.IdMenu.Value;

            pantalla.FechaModificacion = DateTime.Now;
            pantalla.IpModificacion = "IP.PRUEBA";

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PantallaExistsAsync(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeletePantallaAsync(int id)
        {
            var pantalla = await _context.Pantallas.FindAsync(id);

            if (pantalla == null)
            {
                return false;
            }

            // Soft delete
            pantalla.Removido = true;
            pantalla.FechaEliminacion = DateTime.Now;
            pantalla.IpEliminacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PantallaExistsAsync(int id)
        {
            return await _context.Pantallas.AnyAsync(p => p.Id == id && !p.Removido);
        }

        public async Task<IEnumerable<PantallaDTO>> GetPantallasByMenuIdAsync(int menuId)
        {
            var pantallas = await _context.Pantallas
                .Where(p => p.IdMenu == menuId && !p.Removido)
                .Include(p => p.Menu)
                .ToListAsync();

            return pantallas.Select(p => MapPantallaToDTO(p));
        }

        #region Métodos privados

        private PantallaDTO MapPantallaToDTO(Pantalla pantalla)
        {
            return new PantallaDTO
            {
                Id = pantalla.Id,
                Nombre = pantalla.Nombre,
                Nemonico = pantalla.Nemonico,
                Descripcion = pantalla.Descripcion,
                VerificaPermiso = pantalla.VerificaPermiso,
                IdMenu = pantalla.IdMenu,
                Menu = pantalla.Menu != null ? new MenuDTO
                {
                    Id = pantalla.Menu.Id,
                    Nombre = pantalla.Menu.Nombre
                } : null
            };
        }

        #endregion
    }
}