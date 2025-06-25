using ISit_API.Data;
using iSit_API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using iSit_API.Services.Interfaces;
using iSit_API.Models.Entities;

namespace iSit_API.Services.Implementations
{
    public class EmpresaService : IEmpresaService
    {
        private readonly ApplicationDbContext _context;

        public EmpresaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmpresaDTO>> GetAllEmpresasAsync()
        {
            var empresas = await _context.Empresas
                .Where(e => !e.Removido)
                .ToListAsync();

            return empresas.Select(e => new EmpresaDTO
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Nemonico = e.Nemonico
            });
        }

        public async Task<EmpresaDTO> GetEmpresaByIdAsync(int id)
        {
            var empresa = await _context.Empresas
                .Where(e => e.Id == id && !e.Removido)
                .FirstOrDefaultAsync();

            if (empresa == null)
                return null;

            return new EmpresaDTO
            {
                Id = empresa.Id,
                Nombre = empresa.Nombre,
                Nemonico = empresa.Nemonico
            };
        }

        public async Task<EmpresaDTO> CreateEmpresaAsync(CreateEmpresaDTO createEmpresaDto)
        {
            // Verificar si ya existe una empresa con el mismo nombre
            if (await _context.Empresas.AnyAsync(e => e.Nombre == createEmpresaDto.Nombre && !e.Removido))
            {
                throw new InvalidOperationException($"Ya existe una empresa con el nombre '{createEmpresaDto.Nombre}'");
            }

            // Crear nueva entidad Empresa
            var empresa = new Empresa
            {
                Nombre = createEmpresaDto.Nombre,
                Nemonico = createEmpresaDto.Nemonico,
                FechaCreacion = DateTime.Now,
                IpCreacion = "IP.PRUEBA",
                Removido = false
            };

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            return new EmpresaDTO
            {
                Id = empresa.Id,
                Nombre = empresa.Nombre,
                Nemonico = empresa.Nemonico
            };
        }

        public async Task<bool> UpdateEmpresaAsync(int id, UpdateEmpresaDTO updateEmpresaDto)
        {
            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null || empresa.Removido)
            {
                return false;
            }

            // Verificar si el nombre ya existe (si se está actualizando)
            if (!string.IsNullOrEmpty(updateEmpresaDto.Nombre) &&
                updateEmpresaDto.Nombre != empresa.Nombre &&
                await _context.Empresas.AnyAsync(e => e.Nombre == updateEmpresaDto.Nombre && !e.Removido))
            {
                throw new InvalidOperationException($"Ya existe una empresa con el nombre '{updateEmpresaDto.Nombre}'");
            }

            // Actualizar solo los campos proporcionados
            if (!string.IsNullOrEmpty(updateEmpresaDto.Nombre))
                empresa.Nombre = updateEmpresaDto.Nombre;

            if (!string.IsNullOrEmpty(updateEmpresaDto.Nemonico))
                empresa.Nemonico = updateEmpresaDto.Nemonico;

            empresa.FechaModificacion = DateTime.Now;
            empresa.IpModificacion = "IP.PRUEBA";

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EmpresaExistsAsync(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteEmpresaAsync(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
            {
                return false;
            }

            // Soft delete
            empresa.Removido = true;
            empresa.FechaEliminacion = DateTime.Now;
            empresa.IpEliminacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmpresaExistsAsync(int id)
        {
            return await _context.Empresas.AnyAsync(e => e.Id == id && !e.Removido);
        }
    }
}