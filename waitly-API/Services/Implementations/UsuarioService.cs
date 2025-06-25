using waitly_API.Data;
using Microsoft.EntityFrameworkCore;
using waitly_API.Models.DTOs;
using System.Text;
using System.Security.Cryptography;
using waitly_API.Services.Interfaces;
using waitly_API.Models.Entities;

namespace waitly_API.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public UsuarioService(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public AuthResponseDTO GenerateTokenAsync(UsuarioDTO usuario)
        {
            var token = _jwtService.GenerateToken(usuario);

            return new AuthResponseDTO
            {
                Token = token,
                Expiration = DateTime.Now.AddMinutes(60),
                Usuario = usuario
            };
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAllUsuariosAsync()
        {
            var usuarios = await _context.Usuarios
                 .Where(u => !u.Removido)
                 .Include(u => u.UsuarioRoles)
                     .ThenInclude(ur => ur.Rol)
                 .Include(u => u.UsuarioEmpresas)
                     .ThenInclude(ue => ue.Empresa)
                 .ToListAsync();

            return usuarios.Select(u => new UsuarioDTO
            {
                Id = u.Id,
                User = u.User,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Roles = u.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList(),
                Empresas = u.UsuarioEmpresas.Select(ue => new EmpresaDTO
                {
                    Id = ue.Empresa.Id,
                    Nombre = ue.Empresa.Nombre
                }).ToList()
            });
        }

        public async Task<UsuarioDTO> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Id == id && !u.Removido)
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .Include(u => u.UsuarioEmpresas)
                    .ThenInclude(ue => ue.Empresa)
                .FirstOrDefaultAsync();

            if (usuario == null)
                return null;

            return new UsuarioDTO
            {
                Id = usuario.Id,
                User = usuario.User,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Roles = usuario.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList(),
                Empresas = usuario.UsuarioEmpresas.Select(ue => new EmpresaDTO
                {
                    Id = ue.Empresa.Id,
                    Nombre = ue.Empresa.Nombre
                }).ToList()
            };
        }

        public async Task<AuthResponseDTO> CreateUsuarioAsync(CreateUsuarioDTO createUsuarioDto)
        {
            // Verificar si el nombre de usuario ya existe
            if (await _context.Usuarios.AnyAsync(u => u.User == createUsuarioDto.User && !u.Removido))
            {
                throw new InvalidOperationException($"El nombre de usuario '{createUsuarioDto.User}' ya está en uso");
            }

            // Crear nueva entidad Usuario
            var usuario = new Usuario
            {
                User = createUsuarioDto.User,
                PasswordHash = HashPassword(createUsuarioDto.Password),
                Nombres = createUsuarioDto.Nombres,
                Apellidos = createUsuarioDto.Apellidos,
                FechaCreacion = DateTime.Now,
                IpCreacion = "IP.PRUEBA",
                Removido = false,
                UsuarioRoles = new List<UsuarioRol>(),
                UsuarioEmpresas = new List<UsuarioEmpresa>()
            };

            // Asignar rol por defecto (asumiendo que existe un rol "Usuario" con Id=1)
            var rolUsuario = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Usuario");
            if (rolUsuario != null)
            {
                usuario.UsuarioRoles.Add(new UsuarioRol
                {
                    IdRol = rolUsuario.Id,
                    IdUsuario = usuario.Id
                });
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Obtener el usuario completo con sus relaciones
            var usuarioCompleto = await GetUsuarioByIdAsync(usuario.Id);

            // Generar token
            return GenerateTokenAsync(usuarioCompleto);
        }

        public async Task<bool> UpdateUsuarioAsync(int id, UpdateUsuarioDTO updateUsuarioDto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null || usuario.Removido)
            {
                return false;
            }

            // Verificar si el nombre de usuario ya existe (si se está actualizando)
            if (!string.IsNullOrEmpty(updateUsuarioDto.User) &&
                updateUsuarioDto.User != usuario.User &&
                await _context.Usuarios.AnyAsync(u => u.User == updateUsuarioDto.User && !u.Removido))
            {
                throw new InvalidOperationException($"El nombre de usuario '{updateUsuarioDto.User}' ya está en uso");
            }

            // Actualizar solo los campos proporcionados
            if (!string.IsNullOrEmpty(updateUsuarioDto.User))
                usuario.User = updateUsuarioDto.User;

            if (!string.IsNullOrEmpty(updateUsuarioDto.Nombres))
                usuario.Nombres = updateUsuarioDto.Nombres;

            if (!string.IsNullOrEmpty(updateUsuarioDto.Apellidos))
                usuario.Apellidos = updateUsuarioDto.Apellidos;

            if (!string.IsNullOrEmpty(updateUsuarioDto.Password))
                usuario.PasswordHash = HashPassword(updateUsuarioDto.Password);

            usuario.FechaModificacion = DateTime.Now;
            usuario.IpModificacion = "IP.PRUEBA";

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UsuarioExistsAsync(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return false;
            }

            // Soft delete
            usuario.Removido = true;
            usuario.FechaEliminacion = DateTime.Now;
            usuario.IpEliminacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UsuarioExistsAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(e => e.Id == id && !e.Removido);
        }

        public async Task<AuthResponseDTO> AuthenticateAsync(LoginUsuarioDTO loginDto)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.User == loginDto.User && !u.Removido)
                .FirstOrDefaultAsync();

            if (usuario == null || !VerifyPassword(loginDto.Password, usuario.PasswordHash))
            {
                return null;
            }

            // Obtener el usuario completo con sus relaciones
            var usuarioDto = await GetUsuarioByIdAsync(usuario.Id);

            // Generar token
            return GenerateTokenAsync(usuarioDto);
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO changePasswordDto)
        {
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario == null || usuario.Removido)
            {
                return false;
            }

            if (!VerifyPassword(changePasswordDto.CurrentPassword, usuario.PasswordHash))
            {
                throw new InvalidOperationException("La contraseña actual es incorrecta");
            }

            usuario.PasswordHash = HashPassword(changePasswordDto.NewPassword);
            usuario.FechaModificacion = DateTime.Now;
            usuario.IpModificacion = "IP.PRUEBA";

            await _context.SaveChangesAsync();
            return true;
        }

        #region Métodos privados para manejo de contraseñas

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            string passwordHash = HashPassword(password);
            return passwordHash == storedHash;
        }

        #endregion
    }
}
