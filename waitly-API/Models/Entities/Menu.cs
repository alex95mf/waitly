using System.ComponentModel.DataAnnotations;

namespace iSit_API.Models.Entities
{
    public class Menu : EntidadBase
    {
        public int Id { get; set; }
        public int? IdPadre { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }
        public bool VerificaPermiso { get; set; }
        public int Orden { get; set; }
        public string Url { get; set; }
        public int IdTipo { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdPermiso { get; set; }

        // Navigation properties
        public Menu MenuPadre { get; set; }
        public ICollection<Menu> SubMenus { get; set; }
        public Empresa Empresa { get; set; }
        public Permiso Permiso { get; set; }
        public Catalogo Tipo { get; set; }
        public ICollection<Pantalla> Pantallas { get; set; }
    }
}
