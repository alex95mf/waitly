using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.Entities
{
    public class Carta : EntidadBase
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public int IdEmpresa { get; set; }

        // Navigation properties
        public Empresa Empresa { get; set; }

        public ICollection<ItemCarta> ItemsCarta { get; set; }
    }
    
}
