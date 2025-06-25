using System.ComponentModel.DataAnnotations;

namespace iSit_API.Models.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string User { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Apellidos { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
        public List<EmpresaDTO> Empresas { get; set; } = new List<EmpresaDTO>();

    }

    public class CreateUsuarioDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string User { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Apellidos { get; set; }
    }

    public class UpdateUsuarioDTO
    {
        [StringLength(50, MinimumLength = 3)]
        public string User { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string Nombres { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string Apellidos { get; set; }

        [StringLength(30, MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class LoginUsuarioDTO
    {
        [Required]
        [StringLength(50)]
        public string User { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }
    }

    public class ChangePasswordDTO
    {
        [Required]
        [StringLength(30)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
