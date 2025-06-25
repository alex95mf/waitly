using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.DTOs
{
    public class CatalogoDTO
    {
        public int Id { get; set; }
        public int? IdPadre { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Nemonico { get; set; }
        public int Valor { get; set; }
        public string NombrePadre { get; set; }
        public List<CatalogoDTO> Hijos { get; set; } = new List<CatalogoDTO>();
    }

    public class CreateCatalogoDTO
    {
        public int? IdPadre { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [StringLength(50)]
        public string Nemonico { get; set; }

        public int Valor { get; set; }
    }

    public class UpdateCatalogoDTO
    {
        public int? IdPadre { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [StringLength(50)]
        public string Nemonico { get; set; }

        public int? Valor { get; set; }
    }
}