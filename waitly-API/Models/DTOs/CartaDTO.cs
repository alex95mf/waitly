using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.DTOs
{
    public class CartaDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public EmpresaDTO Empresa { get; set; }

        public List<ItemCartaDTO> ItemsCarta { get; set; }

    }

    public class CreateCartaDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public int IdEmpresa { get; set; }
    }

    public class UpdateCartaDTO
    {
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public int? IdEmpresa { get; set; }
    }
}
