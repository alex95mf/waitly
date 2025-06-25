using iSit_API.Models.DTOs;
using iSit_API.Models.Entities;
using iSit_API.Services.Interfaces;
using ISit_API.Data;
using Microsoft.EntityFrameworkCore;

namespace iSit_API.Services.Implementations
{
    public class CatalogoService : ICatalogoService
    {
        private readonly ApplicationDbContext _context;

        public CatalogoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CatalogoDTO>> GetAllCatalogosAsync()
        {
            var catalogos = await _context.Catalogos
                .Where(c => !c.Removido && c.Id != 1) // Excluir el catálogo raíz
                .Include(c => c.Padre)
                .Include(c => c.Hijos.Where(h => !h.Removido))
                .ToListAsync();

            return catalogos.Select(c => MapCatalogoToDTO(c, false));
        }

        public async Task<CatalogoDTO> GetCatalogoByIdAsync(int id)
        {
            var catalogo = await _context.Catalogos
                .Where(c => c.Id == id && !c.Removido)
                .Include(c => c.Padre)
                .Include(c => c.Hijos.Where(h => !h.Removido))
                .FirstOrDefaultAsync();

            if (catalogo == null)
                return null;

            // Filtrar el catálogo raíz si es necesario
            if (id == 1)
            {
                // Para el catálogo raíz, excluimos a él mismo de sus hijos
                catalogo.Hijos = catalogo.Hijos.Where(h => h.Id != 1).ToList();
            }

            return MapCatalogoToDTO(catalogo, true);
        }

        public async Task<IEnumerable<CatalogoDTO>> GetCatalogosByPadreIdAsync(int? padreId)
        {
            // Si padreId es null, obtenemos los catalogos raíz (sin padre)
            // De lo contrario, obtenemos los catalogos hijos del padre especificado
            var query = _context.Catalogos
                .Where(c => !c.Removido);

            if (padreId.HasValue)
            {
                // Verificar si el catálogo padre existe
                var padreExists = await _context.Catalogos.AnyAsync(c => c.Id == padreId.Value && !c.Removido);
                if (!padreExists)
                {
                    throw new InvalidOperationException($"Catálogo padre con ID {padreId.Value} no encontrado");
                }

                query = query.Where(c => c.IdPadre == padreId.Value);
            }
            else
            {
                // Buscar catálogos de primer nivel (hijos del raíz)
                query = query.Where(c => c.IdPadre == 1);
            }

            // Filtramos el catálogo raíz de los resultados
            query = query.Where(c => c.Id != 1);

            var catalogos = await query
                .Include(c => c.Padre)
                .Include(c => c.Hijos.Where(h => !h.Removido))
                .ToListAsync();

            return catalogos.Select(c => MapCatalogoToDTO(c, false));
        }

        public async Task<CatalogoDTO> CreateCatalogoAsync(CreateCatalogoDTO createCatalogoDto)
        {
            // Para catálogos raíz, aseguramos que IdPadre sea 0
            if (!createCatalogoDto.IdPadre.HasValue || createCatalogoDto.IdPadre.Value <= 0)
            {
                createCatalogoDto.IdPadre = 1; // ID del catálogo raíz
            }
            else
            {
                // Verificar si el padre existe
                var padreExists = await _context.Catalogos.AnyAsync(c => c.Id == createCatalogoDto.IdPadre.Value && !c.Removido);
                if (!padreExists)
                {
                    throw new InvalidOperationException($"Catálogo padre con ID {createCatalogoDto.IdPadre.Value} no encontrado");
                }
            }

            // Verificar si ya existe un catálogo con el mismo nemónico
            if (!string.IsNullOrEmpty(createCatalogoDto.Nemonico))
            {
                var nemonicoExistente = await _context.Catalogos
                    .AnyAsync(c => c.Nemonico == createCatalogoDto.Nemonico && !c.Removido);

                if (nemonicoExistente)
                {
                    throw new InvalidOperationException($"Ya existe un catálogo con el nemónico '{createCatalogoDto.Nemonico}'");
                }
            }

            // Verificar si ya existe un catálogo con el mismo nombre y el mismo padre
            var nombreExistente = await _context.Catalogos
                .AnyAsync(c => c.Nombre == createCatalogoDto.Nombre &&
                              c.IdPadre == (createCatalogoDto.IdPadre ?? 0) &&
                              !c.Removido);

            if (nombreExistente)
            {
                throw new InvalidOperationException($"Ya existe un catálogo con el nombre '{createCatalogoDto.Nombre}' en el mismo nivel");
            }

            // Crear nuevo catálogo
            var catalogo = new Catalogo
            {
                IdPadre = createCatalogoDto.IdPadre.Value,
                Nombre = createCatalogoDto.Nombre,
                Descripcion = createCatalogoDto.Descripcion,
                Nemonico = createCatalogoDto.Nemonico,
                Valor = createCatalogoDto.Valor,
                FechaCreacion = DateTime.Now,
                IpCreacion = "IP.PRUEBA",
                Removido = false
            };

            _context.Catalogos.Add(catalogo);
            await _context.SaveChangesAsync();

            // Obtener el catálogo completo con sus relaciones
            return await GetCatalogoByIdAsync(catalogo.Id);
        }

        public async Task<bool> UpdateCatalogoAsync(int id, UpdateCatalogoDTO updateCatalogoDto)
        {
            var catalogo = await _context.Catalogos.FindAsync(id);

            if (catalogo == null || catalogo.Removido)
            {
                return false;
            }

            // Verificar si el padre existe (si se proporciona)
            if (updateCatalogoDto.IdPadre.HasValue && updateCatalogoDto.IdPadre.Value != 0)
            {
                // Evitar ciclos: un catálogo no puede ser su propio padre o descendiente
                if (updateCatalogoDto.IdPadre.Value == id)
                {
                    throw new InvalidOperationException("Un catálogo no puede ser su propio padre");
                }

                // Verificar si el nuevo padre existe
                var padreExists = await _context.Catalogos.AnyAsync(c => c.Id == updateCatalogoDto.IdPadre.Value && !c.Removido);
                if (!padreExists)
                {
                    throw new InvalidOperationException($"Catálogo padre con ID {updateCatalogoDto.IdPadre.Value} no encontrado");
                }

                // Verificar que el nuevo padre no sea un descendiente del catálogo actual (evitar ciclos)
                bool EsDescendiente(int posibleDescendienteId, int ancestroId)
                {
                    var posibleDescendiente = _context.Catalogos.Find(posibleDescendienteId);
                    if (posibleDescendiente == null || posibleDescendiente.IdPadre == 0)
                        return false;
                    if (posibleDescendiente.IdPadre == ancestroId)
                        return true;
                    return EsDescendiente(posibleDescendiente.IdPadre, ancestroId);
                }

                if (EsDescendiente(updateCatalogoDto.IdPadre.Value, id))
                {
                    throw new InvalidOperationException("No se puede asignar como padre a un descendiente");
                }
            }

            // Verificar si ya existe un catálogo con el mismo nemónico
            if (!string.IsNullOrEmpty(updateCatalogoDto.Nemonico) &&
                updateCatalogoDto.Nemonico != catalogo.Nemonico)
            {
                var nemonicoExistente = await _context.Catalogos
                    .AnyAsync(c => c.Nemonico == updateCatalogoDto.Nemonico && c.Id != id && !c.Removido);

                if (nemonicoExistente)
                {
                    throw new InvalidOperationException($"Ya existe un catálogo con el nemónico '{updateCatalogoDto.Nemonico}'");
                }
            }

            // Verificar si ya existe un catálogo con el mismo nombre y el mismo padre
            if (!string.IsNullOrEmpty(updateCatalogoDto.Nombre) &&
                updateCatalogoDto.Nombre != catalogo.Nombre)
            {
                var nombreExistente = await _context.Catalogos
                    .AnyAsync(c => c.Nombre == updateCatalogoDto.Nombre &&
                                  c.IdPadre == (updateCatalogoDto.IdPadre ?? catalogo.IdPadre) &&
                                  c.Id != id &&
                                  !c.Removido);

                if (nombreExistente)
                {
                    throw new InvalidOperationException($"Ya existe un catálogo con el nombre '{updateCatalogoDto.Nombre}' en el mismo nivel");
                }
            }

            // Actualizar campos proporcionados
            if (!string.IsNullOrEmpty(updateCatalogoDto.Nombre))
                catalogo.Nombre = updateCatalogoDto.Nombre;

            if (!string.IsNullOrEmpty(updateCatalogoDto.Descripcion))
                catalogo.Descripcion = updateCatalogoDto.Descripcion;

            if (!string.IsNullOrEmpty(updateCatalogoDto.Nemonico))
                catalogo.Nemonico = updateCatalogoDto.Nemonico;

            if (updateCatalogoDto.Valor.HasValue)
                catalogo.Valor = updateCatalogoDto.Valor.Value;

            if (updateCatalogoDto.IdPadre.HasValue)
                catalogo.IdPadre = updateCatalogoDto.IdPadre.Value;

            catalogo.FechaModificacion = DateTime.Now;
            catalogo.IpModificacion = "IP.PRUEBA";

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CatalogoExistsAsync(id))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteCatalogoAsync(int id)
        {
            var catalogo = await _context.Catalogos
                .Include(c => c.Hijos)
                .FirstOrDefaultAsync(c => c.Id == id && !c.Removido);

            if (catalogo == null)
            {
                return false;
            }

            // Verificar si el catálogo está en uso
            bool catalogoEnUso = await _context.Permisos.AnyAsync(p => p.IdTipo == id && !p.Removido) ||
                                 await _context.Menus.AnyAsync(m => m.IdTipo == id && !m.Removido) ||
                                 await _context.Asientos.AnyAsync(a => a.IdTipo == id || a.IdEstado == id && !a.Removido);

            if (catalogoEnUso)
            {
                throw new InvalidOperationException("No se puede eliminar el catálogo porque está en uso");
            }

            // Verificar si tiene hijos activos
            if (catalogo.Hijos.Any(h => !h.Removido))
            {
                throw new InvalidOperationException("No se puede eliminar el catálogo porque tiene subcatálogos activos");
            }

            // Borrado lógico
            catalogo.Removido = true;
            catalogo.FechaEliminacion = DateTime.Now;
            catalogo.IpEliminacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CatalogoExistsAsync(int id)
        {
            return await _context.Catalogos.AnyAsync(c => c.Id == id && !c.Removido);
        }

        // Método auxiliar para mapear un catálogo a su DTO
        private CatalogoDTO MapCatalogoToDTO(Catalogo catalogo, bool includeHijos)
        {
            var dto = new CatalogoDTO
            {
                Id = catalogo.Id,
                IdPadre = catalogo.IdPadre == 0 ? null : catalogo.IdPadre,
                Nombre = catalogo.Nombre,
                Descripcion = catalogo.Descripcion,
                Nemonico = catalogo.Nemonico,
                Valor = catalogo.Valor,
                NombrePadre = catalogo.Padre?.Nombre ?? string.Empty
            };

            if (includeHijos && catalogo.Hijos != null)
            {
                dto.Hijos = catalogo.Hijos
                    .Where(h => !h.Removido)
                    .Select(h => MapCatalogoToDTO(h, false)) // Evitamos recursión infinita
                    .ToList();
            }

            return dto;
        }
    }
}