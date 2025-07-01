using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.DTOs
{
    public class ItemCartaDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public double Precio { get; set; }

        public CatalogoDTO Categoria { get; set; }
        public CartaDTO Carta { get; set; } 
    }

    public class CreateItemCartaDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public double Precio { get; set; }

        public int IdCategoria { get; set; }

        public int IdCarta { get; set; }
    }

    public class UpdateItemCartaDTO
    {
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public double? Precio { get; set; }

        public int? IdCategoria { get; set; }

        public int? IdCarta { get; set; }
    }
}
