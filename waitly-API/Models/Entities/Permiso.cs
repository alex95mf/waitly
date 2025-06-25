using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.Entities
{
    public class Permiso : EntidadBase
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }
        public int IdTipo { get; set; }

        // Navigation properties
        public ICollection<RolPermiso> RolPermisos { get; set; }
        public ICollection<PermisoPantalla> PermisosPantallas { get; set; }
        public ICollection<Menu> Menus { get; set; }
        public Catalogo Tipo { get; set; }
    }    

    public class PermisoPantalla
    {
        public int Id { get; set; }
        public int IdPermiso { get; set; }
        public int IdPantalla { get; set; }

        // Navigation properties
        public Permiso Permiso { get; set; }
        public Pantalla Pantalla { get; set; }
    }
}
