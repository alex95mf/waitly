using System.ComponentModel.DataAnnotations;

namespace iSit_API.Models.Entities
{
    public class Rol : EntidadBase
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }
        public int IdEmpresa { get; set; }

        // Navigation properties
        public Empresa Empresa { get; set; }
        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
        public ICollection<RolPermiso> RolPermisos { get; set; }
    }

    public class RolPermiso
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public int IdPermiso { get; set; }

        // Navigation properties
        public Rol Rol { get; set; }
        public Permiso Permiso { get; set; }
    }
}
