using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using waitly_API.Data;
using Microsoft.EntityFrameworkCore;

namespace waitly_API.Services.Implementations
{
    public class ItemCartaService : IItemCartaService
    {
        private readonly ApplicationDbContext _context;

        public ItemCartaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemCartaDTO>> GetAllItemsCartaAsync()
        {
            var itemsCarta = await _context.ItemsCarta
                .Include(ic => ic.Carta)
                    .ThenInclude(c => c.Empresa)
                .Include(ic => ic.Categoria)
                .Where(ic => !ic.Removido)
                .ToListAsync();

            return itemsCarta.Select(MapToItemCartaDTO);
        }

        public async Task<ItemCartaDTO> GetItemCartaByIdAsync(int id)
        {
            var itemCarta = await _context.ItemsCarta
                .Include(ic => ic.Carta)
                    .ThenInclude(c => c.Empresa)
                .Include(ic => ic.Categoria)
                .FirstOrDefaultAsync(ic => ic.Id == id && !ic.Removido);

            if (itemCarta == null)
                return null;

            return MapToItemCartaDTO(itemCarta);
        }

        public async Task<ItemCartaDTO> CreateItemCartaAsync(CreateItemCartaDTO createItemCartaDto)
        {
            // Validar que la carta existe
            var cartaExists = await _context.Cartas.AnyAsync(c => c.Id == createItemCartaDto.IdCarta && !c.Removido);
            if (!cartaExists)
            {
                throw new InvalidOperationException($"Carta con ID {createItemCartaDto.IdCarta} no encontrada");
            }

            // Validar que la categoría existe
            var categoriaExists = await _context.Catalogos.AnyAsync(c => c.Id == createItemCartaDto.IdCategoria && !c.Removido);
            if (!categoriaExists)
            {
                throw new InvalidOperationException($"Categoría con ID {createItemCartaDto.IdCategoria} no encontrada");
            }

            // Validar que el precio sea válido
            if (createItemCartaDto.Precio < 0)
            {
                throw new InvalidOperationException("El precio no puede ser negativo");
            }

            var itemCarta = new ItemCarta
            {
                Nombre = createItemCartaDto.Nombre,
                Nemonico = createItemCartaDto.Nemonico,
                Descripcion = createItemCartaDto.Descripcion,
                Precio = createItemCartaDto.Precio,
                IdCategoria = createItemCartaDto.IdCategoria,
                IdCarta = createItemCartaDto.IdCarta,
                FechaCreacion = DateTime.Now,
                IpCreacion = "IP.PRUEBA",
                Removido = false
            };

            _context.ItemsCarta.Add(itemCarta);
            await _context.SaveChangesAsync();

            return await GetItemCartaByIdAsync(itemCarta.Id);
        }

        public async Task<bool> UpdateItemCartaAsync(int id, UpdateItemCartaDTO updateItemCartaDto)
        {
            var itemCarta = await _context.ItemsCarta.FindAsync(id);

            if (itemCarta == null || itemCarta.Removido)
            {
                return false;
            }

            // Validar que la carta existe, si se especifica
            if (updateItemCartaDto.IdCarta.HasValue)
            {
                var cartaExists = await _context.Cartas.AnyAsync(c => c.Id == updateItemCartaDto.IdCarta.Value && !c.Removido);
                if (!cartaExists)
                {
                    throw new InvalidOperationException($"Carta con ID {updateItemCartaDto.IdCarta.Value} no encontrada");
                }
                itemCarta.IdCarta = updateItemCartaDto.IdCarta.Value;
            }

            // Validar que la categoría existe, si se especifica
            if (updateItemCartaDto.IdCategoria.HasValue)
            {
                var categoriaExists = await _context.Catalogos.AnyAsync(c => c.Id == updateItemCartaDto.IdCategoria.Value && !c.Removido);
                if (!categoriaExists)
                {
                    throw new InvalidOperationException($"Categoría con ID {updateItemCartaDto.IdCategoria.Value} no encontrada");
                }
                itemCarta.IdCategoria = updateItemCartaDto.IdCategoria.Value;
            }

            // Validar que el precio sea válido, si se especifica
            if (updateItemCartaDto.Precio.HasValue && updateItemCartaDto.Precio.Value < 0)
            {
                throw new InvalidOperationException("El precio no puede ser negativo");
            }

            // Actualizar propiedades del item
            if (!string.IsNullOrEmpty(updateItemCartaDto.Nombre))
                itemCarta.Nombre = updateItemCartaDto.Nombre;

            if (!string.IsNullOrEmpty(updateItemCartaDto.Nemonico))
                itemCarta.Nemonico = updateItemCartaDto.Nemonico;

            if (!string.IsNullOrEmpty(updateItemCartaDto.Descripcion))
                itemCarta.Descripcion = updateItemCartaDto.Descripcion;

            if (updateItemCartaDto.Precio.HasValue)
                itemCarta.Precio = updateItemCartaDto.Precio.Value;

            itemCarta.FechaModificacion = DateTime.Now;
            itemCarta.IpModificacion = "IP.PRUEBA";

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ItemCartaExistsAsync(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteItemCartaAsync(int id)
        {
            var itemCarta = await _context.ItemsCarta
                .FirstOrDefaultAsync(ic => ic.Id == id && !ic.Removido);

            if (itemCarta == null)
            {
                return false;
            }

            // Marcar como eliminado (soft delete)
            itemCarta.Removido = true;
            itemCarta.FechaEliminacion = DateTime.Now;
            itemCarta.IpEliminacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ItemCartaExistsAsync(int id)
        {
            return await _context.ItemsCarta.AnyAsync(ic => ic.Id == id && !ic.Removido);
        }

        public async Task<IEnumerable<ItemCartaDTO>> GetItemsCartaByCartaIdAsync(int cartaId)
        {
            var cartaExists = await _context.Cartas.AnyAsync(c => c.Id == cartaId && !c.Removido);
            if (!cartaExists)
            {
                throw new InvalidOperationException($"Carta con ID {cartaId} no encontrada");
            }

            var itemsCarta = await _context.ItemsCarta
                .Include(ic => ic.Carta)
                    .ThenInclude(c => c.Empresa)
                .Include(ic => ic.Categoria)
                .Where(ic => ic.IdCarta == cartaId && !ic.Removido)
                .ToListAsync();

            return itemsCarta.Select(MapToItemCartaDTO);
        }

        public async Task<IEnumerable<ItemCartaDTO>> GetItemsCartaByCategoriaIdAsync(int categoriaId)
        {
            var categoriaExists = await _context.Catalogos.AnyAsync(c => c.Id == categoriaId && !c.Removido);
            if (!categoriaExists)
            {
                throw new InvalidOperationException($"Categoría con ID {categoriaId} no encontrada");
            }

            var itemsCarta = await _context.ItemsCarta
                .Include(ic => ic.Carta)
                    .ThenInclude(c => c.Empresa)
                .Include(ic => ic.Categoria)
                .Where(ic => ic.IdCategoria == categoriaId && !ic.Removido)
                .ToListAsync();

            return itemsCarta.Select(MapToItemCartaDTO);
        }

        public async Task<IEnumerable<ItemCartaDTO>> GetItemsCartaByCartaAndCategoriaAsync(int cartaId, int categoriaId)
        {
            // Validar que la carta existe
            var cartaExists = await _context.Cartas.AnyAsync(c => c.Id == cartaId && !c.Removido);
            if (!cartaExists)
            {
                throw new InvalidOperationException($"Carta con ID {cartaId} no encontrada");
            }

            // Validar que la categoría existe
            var categoriaExists = await _context.Catalogos.AnyAsync(c => c.Id == categoriaId && !c.Removido);
            if (!categoriaExists)
            {
                throw new InvalidOperationException($"Categoría con ID {categoriaId} no encontrada");
            }

            var itemsCarta = await _context.ItemsCarta
                .Include(ic => ic.Carta)
                    .ThenInclude(c => c.Empresa)
                .Include(ic => ic.Categoria)
                .Where(ic => ic.IdCarta == cartaId && ic.IdCategoria == categoriaId && !ic.Removido)
                .ToListAsync();

            return itemsCarta.Select(MapToItemCartaDTO);
        }

        #region Métodos privados

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
                    IdPadre = itemCarta.Categoria.IdPadre == 1 ? null : itemCarta.Categoria.IdPadre,
                    NombrePadre = itemCarta.Categoria.Padre?.Nombre ?? string.Empty
                } : null,
                Carta = itemCarta.Carta != null ? new CartaDTO
                {
                    Nombre = itemCarta.Carta.Nombre,
                    Nemonico = itemCarta.Carta.Nemonico,
                    Descripcion = itemCarta.Carta.Descripcion,
                    Empresa = itemCarta.Carta.Empresa != null ? new EmpresaDTO
                    {
                        Id = itemCarta.Carta.Empresa.Id,
                        Nombre = itemCarta.Carta.Empresa.Nombre,
                        Nemonico = itemCarta.Carta.Empresa.Nemonico
                    } : null,
                    ItemsCarta = new List<ItemCartaDTO>() // Evitar referencia circular
                } : null
            };
        }

        #endregion
    }
}