using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.Entities
{
    public class Asiento : EntidadBase
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }
        public int IdEstado { get; set; }
        public int IdTipo { get; set; }
        public int IdGrupoAsiento { get; set; }

        // Navigation properties
        public GrupoAsiento GrupoAsiento { get; set; }
        public Catalogo Tipo { get; set; }
        public Catalogo Estado { get; set; }

    }
}
