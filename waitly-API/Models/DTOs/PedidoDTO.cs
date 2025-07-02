using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Codigo { get; set; }
        public double Subtotal { get; set; }
        public double Impuesto { get; set; }
        public double Total { get; set; }

        [StringLength(20, MinimumLength = 10)]
        public string IdentificacionCliente { get; set; }

        [StringLength(50)]
        public string RazonSocialCliente { get; set; }

        [StringLength(100)]
        public string NombreComercialCliente { get; set; }

        [StringLength(100)]
        public string NombresCliente { get; set; }

        [StringLength(100)]
        public string ApellidosCliente { get; set; }
        public string Estado { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public List<ItemCartaDTO> ItemsCarta { get; set; }
    }

    public class CreatePedidoDTO
    {
        [Required]
        [StringLength(10)]
        public string Codigo { get; set; }
        public double Subtotal { get; set; }
        public double Impuesto { get; set; }
        public double Total { get; set; }
        [StringLength(20, MinimumLength = 10)]
        public string IdentificacionCliente { get; set; }
        [StringLength(50)]
        public string RazonSocialCliente { get; set; }
        [StringLength(100)]
        public string NombreComercialCliente { get; set; }
        [StringLength(100)]
        public string NombresCliente { get; set; }
        [StringLength(100)]
        public string ApellidosCliente { get; set; }
        public int IdEmpresa { get; set; }
    }

    public class UpdatePedidoDTO
    {
        [StringLength(10)]
        public string Codigo { get; set; }

        public double? Subtotal { get; set; }
        public double? Impuesto { get; set; }
        public double? Total { get; set; }

        [StringLength(20, MinimumLength = 10)]
        public string IdentificacionCliente { get; set; }

        [StringLength(50)]
        public string RazonSocialCliente { get; set; }

        [StringLength(100)]
        public string NombreComercialCliente { get; set; }

        [StringLength(100)]
        public string NombresCliente { get; set; }

        [StringLength(100)]
        public string ApellidosCliente { get; set; }

        public int? IdEstado { get; set; }
        public int? IdEmpresa { get; set; }
        public List<ItemCartaDTO> ItemsCarta { get; set; }
    }

    public class AddItemsToPedidoDTO
    {
        [Required]
        public List<int> ItemsCartaIds { get; set; } = new List<int>();
    }

    public class RemoveItemsFromPedidoDTO
    {
        [Required]
        public List<int> ItemsCartaIds { get; set; } = new List<int>();
    }
}
