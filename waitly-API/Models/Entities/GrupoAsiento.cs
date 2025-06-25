using System.ComponentModel.DataAnnotations;

namespace iSit_API.Models.Entities
{
    public class GrupoAsiento : EntidadBase
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }
        public int Capacidad { get; set; }

        // Navigation properties
        public ICollection<Asiento> Asientos { get; set; }
    }
}
