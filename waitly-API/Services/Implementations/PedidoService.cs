using waitly_API.Models.DTOs;
using waitly_API.Models.Entities;
using waitly_API.Services.Interfaces;
using waitly_API.Data;
using Microsoft.EntityFrameworkCore;

namespace waitly_API.Services.Implementations
{
    public class PedidoService : IPedidoService
    {
        private readonly ApplicationDbContext _context;

        public PedidoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoDTO>> GetAllPedidosAsync()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Empresa)
                .Include(p => p.Estado)
                .Include(p => p.ItemsCarta.Where(ic => !ic.Removido))
                    .ThenInclude(ic => ic.Categoria)
                .Where(p => !p.Removido)
                .ToListAsync();

            return pedidos.Select(MapToPedidoDTO);
        }

        public async Task<PedidoDTO> GetPedidoByIdAsync(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Empresa)
                .Include(p => p.Estado)
                .Include(p => p.ItemsCarta.Where(ic => !ic.Removido))
                    .ThenInclude(ic => ic.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Removido);

            if (pedido == null)
                return null;

            return MapToPedidoDTO(pedido);
        }

        public async Task<PedidoDTO> CreatePedidoAsync(CreatePedidoDTO createPedidoDto)
        {
            // Validar que la empresa existe
            var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == createPedidoDto.IdEmpresa && !e.Removido);
            if (!empresaExists)
            {
                throw new InvalidOperationException($"Empresa con ID {createPedidoDto.IdEmpresa} no encontrada");
            }

            // Buscar el estado PENDIENTE para asignarlo automáticamente
            var estadoPendiente = await _context.Catalogos
                .FirstOrDefaultAsync(c => c.Nemonico == "PENDIENTE" && !c.Removido);

            if (estadoPendiente == null)
            {
                throw new InvalidOperationException("Estado PENDIENTE no encontrado en el catálogo");
            }

            // Validar que el código no exista
            var codigoExists = await _context.Pedidos.AnyAsync(p => p.Codigo == createPedidoDto.Codigo && !p.Removido);
            if (codigoExists)
            {
                throw new InvalidOperationException($"Ya existe un pedido con el código '{createPedidoDto.Codigo}'");
            }

            // Validar totales
            if (createPedidoDto.Subtotal < 0 || createPedidoDto.Impuesto < 0 || createPedidoDto.Total < 0)
            {
                throw new InvalidOperationException("Los montos no pueden ser negativos");
            }

            var pedido = new Pedido
            {
                Codigo = createPedidoDto.Codigo,
                Subtotal = createPedidoDto.Subtotal,
                Impuesto = createPedidoDto.Impuesto,
                Total = createPedidoDto.Total,
                Identificacion = createPedidoDto.IdentificacionCliente,
                RazonSocial = createPedidoDto.RazonSocialCliente,
                NombreComercial = createPedidoDto.NombreComercialCliente,
                Nombres = createPedidoDto.NombresCliente,
                Apellidos = createPedidoDto.ApellidosCliente,
                IdEstado = estadoPendiente.Id, // Automáticamente PENDIENTE
                IdEmpresa = createPedidoDto.IdEmpresa,
                FechaCreacion = DateTime.Now,
                IpCreacion = "IP.PRUEBA",
                Removido = false
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return await GetPedidoByIdAsync(pedido.Id);
        }

        public async Task<bool> UpdatePedidoAsync(int id, UpdatePedidoDTO updatePedidoDto)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null || pedido.Removido)
            {
                return false;
            }

            // Validar que la empresa existe, si se especifica
            if (updatePedidoDto.IdEmpresa.HasValue)
            {
                var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == updatePedidoDto.IdEmpresa.Value && !e.Removido);
                if (!empresaExists)
                {
                    throw new InvalidOperationException($"Empresa con ID {updatePedidoDto.IdEmpresa.Value} no encontrada");
                }
                pedido.IdEmpresa = updatePedidoDto.IdEmpresa.Value;
            }

            // Validar que el estado existe, si se especifica
            if (updatePedidoDto.IdEstado.HasValue)
            {
                var estadoExists = await _context.Catalogos.AnyAsync(c => c.Id == updatePedidoDto.IdEstado.Value && !c.Removido);
                if (!estadoExists)
                {
                    throw new InvalidOperationException($"Estado con ID {updatePedidoDto.IdEstado.Value} no encontrado");
                }
                pedido.IdEstado = updatePedidoDto.IdEstado.Value;
            }

            // Validar que el código no exista en otro pedido, si se especifica
            if (!string.IsNullOrEmpty(updatePedidoDto.Codigo) && updatePedidoDto.Codigo != pedido.Codigo)
            {
                var codigoExists = await _context.Pedidos.AnyAsync(p => p.Codigo == updatePedidoDto.Codigo && p.Id != id && !p.Removido);
                if (codigoExists)
                {
                    throw new InvalidOperationException($"Ya existe un pedido con el código '{updatePedidoDto.Codigo}'");
                }
            }

            // Validar totales
            if (updatePedidoDto.Subtotal.HasValue && updatePedidoDto.Subtotal.Value < 0)
                throw new InvalidOperationException("El subtotal no puede ser negativo");
            if (updatePedidoDto.Impuesto.HasValue && updatePedidoDto.Impuesto.Value < 0)
                throw new InvalidOperationException("El impuesto no puede ser negativo");
            if (updatePedidoDto.Total.HasValue && updatePedidoDto.Total.Value < 0)
                throw new InvalidOperationException("El total no puede ser negativo");

            // Actualizar propiedades del pedido
            if (!string.IsNullOrEmpty(updatePedidoDto.Codigo))
                pedido.Codigo = updatePedidoDto.Codigo;

            if (updatePedidoDto.Subtotal.HasValue)
                pedido.Subtotal = updatePedidoDto.Subtotal.Value;

            if (updatePedidoDto.Impuesto.HasValue)
                pedido.Impuesto = updatePedidoDto.Impuesto.Value;

            if (updatePedidoDto.Total.HasValue)
                pedido.Total = updatePedidoDto.Total.Value;

            if (!string.IsNullOrEmpty(updatePedidoDto.IdentificacionCliente))
                pedido.Identificacion = updatePedidoDto.IdentificacionCliente;

            if (!string.IsNullOrEmpty(updatePedidoDto.RazonSocialCliente))
                pedido.RazonSocial = updatePedidoDto.RazonSocialCliente;

            if (!string.IsNullOrEmpty(updatePedidoDto.NombreComercialCliente))
                pedido.NombreComercial = updatePedidoDto.NombreComercialCliente;

            if (!string.IsNullOrEmpty(updatePedidoDto.NombresCliente))
                pedido.Nombres = updatePedidoDto.NombresCliente;

            if (!string.IsNullOrEmpty(updatePedidoDto.ApellidosCliente))
                pedido.Apellidos = updatePedidoDto.ApellidosCliente;

            pedido.FechaModificacion = DateTime.Now;
            pedido.IpModificacion = "IP.PRUEBA";

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PedidoExistsAsync(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeletePedidoAsync(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.ItemsCarta)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Removido);

            if (pedido == null)
            {
                return false;
            }

            // Verificar si tiene items activos
            if (pedido.ItemsCarta != null && pedido.ItemsCarta.Any(ic => !ic.Removido))
            {
                throw new InvalidOperationException("No se puede eliminar un pedido que tiene items activos");
            }

            // Marcar como eliminado (soft delete)
            pedido.Removido = true;
            pedido.FechaEliminacion = DateTime.Now;
            pedido.IpEliminacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PedidoExistsAsync(int id)
        {
            return await _context.Pedidos.AnyAsync(p => p.Id == id && !p.Removido);
        }

        public async Task<IEnumerable<PedidoDTO>> GetPedidosByEmpresaIdAsync(int empresaId)
        {
            var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == empresaId && !e.Removido);
            if (!empresaExists)
            {
                throw new InvalidOperationException($"Empresa con ID {empresaId} no encontrada");
            }

            var pedidos = await _context.Pedidos
                .Include(p => p.Empresa)
                .Include(p => p.Estado)
                .Include(p => p.ItemsCarta.Where(ic => !ic.Removido))
                    .ThenInclude(ic => ic.Categoria)
                .Where(p => p.IdEmpresa == empresaId && !p.Removido)
                .ToListAsync();

            return pedidos.Select(MapToPedidoDTO);
        }

        public async Task<IEnumerable<PedidoDTO>> GetPedidosByEstadoIdAsync(int estadoId)
        {
            var estadoExists = await _context.Catalogos.AnyAsync(c => c.Id == estadoId && !c.Removido);
            if (!estadoExists)
            {
                throw new InvalidOperationException($"Estado con ID {estadoId} no encontrado");
            }

            var pedidos = await _context.Pedidos
                .Include(p => p.Empresa)
                .Include(p => p.Estado)
                .Include(p => p.ItemsCarta.Where(ic => !ic.Removido))
                    .ThenInclude(ic => ic.Categoria)
                .Where(p => p.IdEstado == estadoId && !p.Removido)
                .ToListAsync();

            return pedidos.Select(MapToPedidoDTO);
        }

        public async Task<IEnumerable<PedidoDTO>> GetPedidosByEmpresaAndEstadoAsync(int empresaId, int estadoId)
        {
            // Validar que la empresa existe
            var empresaExists = await _context.Empresas.AnyAsync(e => e.Id == empresaId && !e.Removido);
            if (!empresaExists)
            {
                throw new InvalidOperationException($"Empresa con ID {empresaId} no encontrada");
            }

            // Validar que el estado existe
            var estadoExists = await _context.Catalogos.AnyAsync(c => c.Id == estadoId && !c.Removido);
            if (!estadoExists)
            {
                throw new InvalidOperationException($"Estado con ID {estadoId} no encontrado");
            }

            var pedidos = await _context.Pedidos
                .Include(p => p.Empresa)
                .Include(p => p.Estado)
                .Include(p => p.ItemsCarta.Where(ic => !ic.Removido))
                    .ThenInclude(ic => ic.Categoria)
                .Where(p => p.IdEmpresa == empresaId && p.IdEstado == estadoId && !p.Removido)
                .ToListAsync();

            return pedidos.Select(MapToPedidoDTO);
        }

        public async Task<PedidoDTO> GetPedidoByCodigoAsync(string codigo)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Empresa)
                .Include(p => p.Estado)
                .Include(p => p.ItemsCarta.Where(ic => !ic.Removido))
                    .ThenInclude(ic => ic.Categoria)
                .FirstOrDefaultAsync(p => p.Codigo == codigo && !p.Removido);

            if (pedido == null)
                return null;

            return MapToPedidoDTO(pedido);
        }

        public async Task<bool> AddItemsToPedidoAsync(int pedidoId, List<int> itemsCartaIds)
        {
            if (itemsCartaIds == null || !itemsCartaIds.Any())
            {
                throw new InvalidOperationException("La lista de items no puede estar vacía");
            }

            var pedido = await _context.Pedidos.FindAsync(pedidoId);
            if (pedido == null || pedido.Removido)
            {
                throw new InvalidOperationException($"Pedido con ID {pedidoId} no encontrado");
            }

            // Verificar que todos los items existan
            var itemsExistentes = await _context.ItemsCarta
                .Where(ic => itemsCartaIds.Contains(ic.Id) && !ic.Removido)
                .ToListAsync();

            if (itemsExistentes.Count != itemsCartaIds.Count)
            {
                var itemsNoEncontrados = itemsCartaIds.Except(itemsExistentes.Select(ie => ie.Id));
                throw new InvalidOperationException($"Items con IDs {string.Join(", ", itemsNoEncontrados)} no encontrados");
            }

            // Verificar si algún item ya está asignado a otro pedido
            var itemsYaAsignados = itemsExistentes
                .Where(ic => ic.IdPedido.HasValue && ic.IdPedido.Value != pedidoId)
                .ToList();

            if (itemsYaAsignados.Any())
            {
                var idsAsignados = itemsYaAsignados.Select(ia => ia.Id);
                throw new InvalidOperationException($"Items con IDs {string.Join(", ", idsAsignados)} ya están asignados a otros pedidos");
            }

            // Asignar todos los items al pedido
            foreach (var item in itemsExistentes)
            {
                item.IdPedido = pedidoId;
                item.FechaModificacion = DateTime.Now;
                item.IpModificacion = "IP.PRUEBA";
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveItemsFromPedidoAsync(int pedidoId, List<int> itemsCartaIds)
        {
            if (itemsCartaIds == null || !itemsCartaIds.Any())
            {
                throw new InvalidOperationException("La lista de items no puede estar vacía");
            }

            var itemsToRemove = await _context.ItemsCarta
                .Where(ic => itemsCartaIds.Contains(ic.Id) && ic.IdPedido == pedidoId && !ic.Removido)
                .ToListAsync();

            if (!itemsToRemove.Any())
            {
                return false;
            }

            // Remover los items del pedido
            foreach (var item in itemsToRemove)
            {
                item.IdPedido = null;
                item.FechaModificacion = DateTime.Now;
                item.IpModificacion = "IP.PRUEBA";
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddItemToPedidoAsync(int pedidoId, int itemCartaId)
        {
            return await AddItemsToPedidoAsync(pedidoId, new List<int> { itemCartaId });
        }

        public async Task<bool> RemoveItemFromPedidoAsync(int pedidoId, int itemCartaId)
        {
            return await RemoveItemsFromPedidoAsync(pedidoId, new List<int> { itemCartaId });
        }

        public async Task<bool> CompletarPedidoAsync(int pedidoId)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Estado)
                .FirstOrDefaultAsync(p => p.Id == pedidoId && !p.Removido);

            if (pedido == null)
            {
                throw new InvalidOperationException($"Pedido con ID {pedidoId} no encontrado");
            }

            // Verificar que el pedido esté en estado PENDIENTE
            if (pedido.Estado?.Nemonico != "PENDIENTE")
            {
                throw new InvalidOperationException("Solo se pueden completar pedidos en estado PENDIENTE");
            }

            // Buscar el estado COMPLETADO
            var estadoCompletado = await _context.Catalogos
                .FirstOrDefaultAsync(c => c.Nemonico == "COMPLETADO" && !c.Removido);

            if (estadoCompletado == null)
            {
                throw new InvalidOperationException("Estado COMPLETADO no encontrado en el catálogo");
            }

            pedido.IdEstado = estadoCompletado.Id;
            pedido.FechaModificacion = DateTime.Now;
            pedido.IpModificacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AnularPedidoAsync(int pedidoId)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Estado)
                .FirstOrDefaultAsync(p => p.Id == pedidoId && !p.Removido);

            if (pedido == null)
            {
                throw new InvalidOperationException($"Pedido con ID {pedidoId} no encontrado");
            }

            // Verificar que el pedido esté en estado PENDIENTE o COMPLETADO
            if (pedido.Estado?.Nemonico != "PENDIENTE" && pedido.Estado?.Nemonico != "COMPLETADO")
            {
                throw new InvalidOperationException("Solo se pueden anular pedidos en estado PENDIENTE o COMPLETADO");
            }

            // Buscar el estado ANULADO
            var estadoAnulado = await _context.Catalogos
                .FirstOrDefaultAsync(c => c.Nemonico == "ANULADO" && !c.Removido);

            if (estadoAnulado == null)
            {
                throw new InvalidOperationException("Estado ANULADO no encontrado en el catálogo");
            }

            pedido.IdEstado = estadoAnulado.Id;
            pedido.FechaModificacion = DateTime.Now;
            pedido.IpModificacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReactivarPedidoAsync(int pedidoId)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Estado)
                .FirstOrDefaultAsync(p => p.Id == pedidoId && !p.Removido);

            if (pedido == null)
            {
                throw new InvalidOperationException($"Pedido con ID {pedidoId} no encontrado");
            }

            // Verificar que el pedido esté en estado ANULADO
            if (pedido.Estado?.Nemonico != "ANULADO")
            {
                throw new InvalidOperationException("Solo se pueden reactivar pedidos en estado ANULADO");
            }

            // Buscar el estado PENDIENTE
            var estadoPendiente = await _context.Catalogos
                .FirstOrDefaultAsync(c => c.Nemonico == "PENDIENTE" && !c.Removido);

            if (estadoPendiente == null)
            {
                throw new InvalidOperationException("Estado PENDIENTE no encontrado en el catálogo");
            }

            pedido.IdEstado = estadoPendiente.Id;
            pedido.FechaModificacion = DateTime.Now;
            pedido.IpModificacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        #region Métodos privados

        private PedidoDTO MapToPedidoDTO(Pedido pedido)
        {
            return new PedidoDTO
            {
                Id = pedido.Id,
                Codigo = pedido.Codigo,
                Subtotal = pedido.Subtotal,
                Impuesto = pedido.Impuesto,
                Total = pedido.Total,
                IdentificacionCliente = pedido.Identificacion,
                RazonSocialCliente = pedido.RazonSocial,
                NombreComercialCliente = pedido.NombreComercial,
                NombresCliente = pedido.Nombres,
                ApellidosCliente = pedido.Apellidos,
                Estado = pedido.Estado?.Nombre ?? string.Empty,
                Empresa = pedido.Empresa != null ? new EmpresaDTO
                {
                    Id = pedido.Empresa.Id,
                    Nombre = pedido.Empresa.Nombre,
                    Nemonico = pedido.Empresa.Nemonico
                } : null,
                ItemsCarta = pedido.ItemsCarta?.Select(MapToItemCartaDTO).ToList() ?? new List<ItemCartaDTO>()
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
                    IdPadre = itemCarta.Categoria.IdPadre == 1 ? null : itemCarta.Categoria.IdPadre,
                    NombrePadre = itemCarta.Categoria.Padre?.Nombre ?? string.Empty
                } : null,
                Carta = null // Evitar referencia circular en el contexto de pedidos
            };
        }

        #endregion
    }
}