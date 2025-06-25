using waitly_API.Models.Auth;
using waitly_API.Models.DTOs;
using waitly_API.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace waitly_API.Services.Implementations
{
    public class JwtService: IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }


        public string GenerateToken(UsuarioDTO usuario)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Crear los claims para el token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.User),
                new Claim("nombres", usuario.Nombres),
                new Claim("apellidos", usuario.Apellidos)
            };

            // Añadir cada rol como un claim separado
            foreach (var rol in usuario.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            // Opcionalmente, añadir las empresas como claims
            if (usuario.Empresas != null && usuario.Empresas.Any())
            {
                // Convertir la lista de IDs de empresas a una cadena separada por comas
                var empresasIds = string.Join(",", usuario.Empresas.Select(e => e.Id));
                claims.Add(new Claim("empresas", empresasIds));
            }

            // Crear el token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
