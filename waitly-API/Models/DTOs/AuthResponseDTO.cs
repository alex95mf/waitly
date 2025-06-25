namespace waitly_API.Models.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UsuarioDTO Usuario { get; set; }
    }
}
