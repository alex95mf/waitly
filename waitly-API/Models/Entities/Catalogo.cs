using System.ComponentModel.DataAnnotations;

namespace iSit_API.Models.Entities
{
    public class Catalogo : EntidadBase
    {
        public int Id { get; set; }
        public int IdPadre { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [StringLength(50)]
        public string Nemonico { get; set; }
        public int Valor { get; set; }

        // Navigation properties
        public Catalogo Padre { get; set; }
        public ICollection<Catalogo> Hijos { get; set; }
    }
}
