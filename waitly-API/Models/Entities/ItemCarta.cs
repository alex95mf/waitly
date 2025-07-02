using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.Entities
{
    public class ItemCarta : EntidadBase
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public int IdCategoria { get; set; }
        public int IdCarta { get; set; }
        public int? IdPedido { get; set; }

        // Navigation properties
        public Catalogo Categoria { get; set; }
        public Carta Carta { get; set; }
        public Pedido Pedido { get; set; }
    }

}
