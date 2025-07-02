using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.Entities
{
    public class Pedido : EntidadBase
    {
        public int Id { get; set; }

        [StringLength(10)]
        public string Codigo { get; set; }        
        public double Subtotal { get; set; }
        public double Impuesto { get; set; }
        public double Total { get; set; }

        [StringLength(20, MinimumLength = 10)]
        public string Identificacion { get; set; }

        [StringLength(50)]
        public string RazonSocial { get; set; }

        [StringLength(100)]
        public string NombreComercial { get; set; }

        [StringLength(100)]
        public string Nombres { get; set; }

        [StringLength(100)]
        public string Apellidos { get; set; }
        public int IdEstado { get; set; }
        public int IdEmpresa { get; set; }

        // Navigation properties
        public Catalogo Estado { get; set; }
        public Empresa Empresa { get; set; }
        public ICollection<ItemCarta> ItemsCarta { get; set; }
    }
    
}
