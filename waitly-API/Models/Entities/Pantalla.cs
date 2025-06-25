using System.ComponentModel.DataAnnotations;

namespace iSit_API.Models.Entities
{
    public class Pantalla : EntidadBase
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }
        public bool VerificaPermiso { get; set; }
        public int IdMenu { get; set; }

        // Navigation properties
        public Menu Menu { get; set; }
        public ICollection<PermisoPantalla> PermisosPantallas { get; set; }
    }
}
