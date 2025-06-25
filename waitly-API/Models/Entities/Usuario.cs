using System.ComponentModel.DataAnnotations;

namespace iSit_API.Models.Entities
{
    public class Usuario : EntidadBase
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string User { get; set; }

        [StringLength(255)]
        public string PasswordHash { get; set; }

        [StringLength(100)]
        public string Nombres { get; set; }

        [StringLength(100)]
        public string Apellidos { get; set; }

        // Navigation properties
        public ICollection<UsuarioEmpresa> UsuarioEmpresas { get; set; }
        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
    }

    public class UsuarioEmpresa
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }

        // Navigation properties
        public Usuario Usuario { get; set; }
        public Empresa Empresa { get; set; }
    }

    public class UsuarioRol
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }

        // Navigation properties
        public Usuario Usuario { get; set; }
        public Rol Rol { get; set; }
    }
}
