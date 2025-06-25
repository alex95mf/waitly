using System.ComponentModel.DataAnnotations;

namespace iSit_API.Models.DTOs
{
    public class PantallaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Nemonico { get; set; }
        public string Descripcion { get; set; }
        public bool VerificaPermiso { get; set; }
        public int IdMenu { get; set; }
        public MenuDTO Menu { get; set; }
    }

    public class CreatePantallaDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public bool VerificaPermiso { get; set; } = true;

        [Required]
        public int IdMenu { get; set; }
    }

    public class UpdatePantallaDTO
    {
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public bool? VerificaPermiso { get; set; }

        public int? IdMenu { get; set; }
    }
}