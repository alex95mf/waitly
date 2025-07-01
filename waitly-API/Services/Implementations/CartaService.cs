using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using waitly_API.Data;
using Microsoft.EntityFrameworkCore;

namespace waitly_API.Services.Implementations
{
    public class CartaService : ICartaService
    {
        private readonly ApplicationDbContext _context;

        public CartaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartaDTO>> GetAllCartasAsync()
        {
            var cartas = await _context.Cartas
                .Include(c => c.Empresa)
                .Include(c => c.ItemsCarta.Where(ic => !ic.Removido))
                    .ThenInclude(ic => ic.Categoria)
                .Where(c => !c.Removido)
                .ToListAsync();

            return cartas.Select(MapToCartaDTO);
        }

        public async Task<CartaDTO> GetCartaByIdAsync(int id)
        {
            var carta = await _context.Cartas
                .Include(c => c.Empresa)
                .Include(c => c.ItemsCarta.Where(ic => !ic.Removido))
                    .ThenInclude(ic => ic.Categoria)
                .FirstOrDefaultAsync(c => c.Id == id && !c.Removido);

            if (carta == null)
                return null;

            return MapToCartaDTO(carta);
        }

        public async Task<CartaDTO> CreateCartaAsync(CreateCartaDTO createCartaDto)
        {
            // Validar que la empresa existe
            var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == createCartaDto.IdEmpresa && !e.Removido);
            if (!empresaExists)
            {
                throw new InvalidOperationException($"Empresa con ID {createCartaDto.IdEmpresa} no encontrada");
            }

            var carta = new Carta
            {
                Nombre = createCartaDto.Nombre,
                Nemonico = createCartaDto.Nemonico,
                Descripcion = createCartaDto.Descripcion,
                IdEmpresa = createCartaDto.IdEmpresa,
                FechaCreacion = DateTime.Now,
                IpCreacion = "IP.PRUEBA",
                Removido = false
            };

            _context.Cartas.Add(carta);
            await _context.SaveChangesAsync();

            return await GetCartaByIdAsync(carta.Id);
        }

        public async Task<bool> UpdateCartaAsync(int id, UpdateCartaDTO updateCartaDto)
        {
            var carta = await _context.Cartas.FindAsync(id);

            if (carta == null || carta.Removido)
            {
                return false;
            }

            // Validar que la empresa existe, si se especifica
            if (updateCartaDto.IdEmpresa.HasValue)
            {
                var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == updateCartaDto.IdEmpresa.Value && !e.Removido);
                if (!empresaExists)
                {
                    throw new InvalidOperationException($"Empresa con ID {updateCartaDto.IdEmpresa.Value} no encontrada");
                }
                carta.IdEmpresa = updateCartaDto.IdEmpresa.Value;
            }

            // Actualizar propiedades de la carta
            if (!string.IsNullOrEmpty(updateCartaDto.Nombre))
                carta.Nombre = updateCartaDto.Nombre;

            if (!string.IsNullOrEmpty(updateCartaDto.Nemonico))
                carta.Nemonico = updateCartaDto.Nemonico;

            if (!string.IsNullOrEmpty(updateCartaDto.Descripcion))
                carta.Descripcion = updateCartaDto.Descripcion;

            carta.FechaModificacion = DateTime.Now;
            carta.IpModificacion = "IP.PRUEBA";

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CartaExistsAsync(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteCartaAsync(int id)
        {
            var carta = await _context.Cartas
                .Include(c => c.ItemsCarta)
                .FirstOrDefaultAsync(c => c.Id == id && !c.Removido);

            if (carta == null)
            {
                return false;
            }

            // Verificar si tiene items activos
            if (carta.ItemsCarta != null && carta.ItemsCarta.Any(ic => !ic.Removido))
            {
                throw new InvalidOperationException("No se puede eliminar una carta que tiene items activos");
            }

            // Marcar como eliminado (soft delete)
            carta.Removido = true;
            carta.FechaEliminacion = DateTime.Now;
            carta.IpEliminacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CartaExistsAsync(int id)
        {
            return await _context.Cartas.AnyAsync(c => c.Id == id && !c.Removido);
        }

        public async Task<IEnumerable<CartaDTO>> GetCartasByEmpresaIdAsync(int empresaId)
        {
            var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == empresaId && !e.Removido);
            if (!empresaExists)
            {
                throw new InvalidOperationException($"Empresa con ID {empresaId} no encontrada");
            }

            var cartas = await _context.Cartas
                .Include(c => c.Empresa)
                .Include(c => c.ItemsCarta.Where(ic => !ic.Removido))
                    .ThenInclude(ic => ic.Categoria)
                .Where(c => c.IdEmpresa == empresaId && !c.Removido)
                .ToListAsync();

            return cartas.Select(MapToCartaDTO);
        }

        #region Métodos privados

        private CartaDTO MapToCartaDTO(Carta carta)
        {
            return new CartaDTO
            {
                Nombre = carta.Nombre,
                Nemonico = carta.Nemonico,
                Descripcion = carta.Descripcion,
                Empresa = carta.Empresa != null ? new EmpresaDTO
                {
                    Id = carta.Empresa.Id,
                    Nombre = carta.Empresa.Nombre,
                    Nemonico = carta.Empresa.Nemonico
                } : null,
                ItemsCarta = carta.ItemsCarta?.Select(MapToItemCartaDTO).ToList() ?? new List<ItemCartaDTO>()
            };
        }

        private ItemCartaDTO MapToItemCartaDTO(ItemCarta itemCarta)
        {
            return new ItemCartaDTO
            {
                Id = itemCarta.Id,
                Nombre = itemCarta.Nombre,
                Nemonico = itemCarta.Nemonico,
                Descripcion = itemCarta.Descripcion,
                Precio = itemCarta.Precio,
                Categoria = itemCarta.Categoria != null ? new CatalogoDTO
                {
                    Id = itemCarta.Categoria.Id,
                    Nombre = itemCarta.Categoria.Nombre,
                    Nemonico = itemCarta.Categoria.Nemonico,
                    Descripcion = itemCarta.Categoria.Descripcion,
                    Valor = itemCarta.Categoria.Valor,
                    NombrePadre = itemCarta.Categoria.Padre.Nombre                    
                } : null,
                Carta = null // Evitar referencia circular
            };
        }

        #endregion
    }
}