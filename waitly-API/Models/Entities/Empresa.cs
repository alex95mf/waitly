using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.Entities
{
    public class Empresa : EntidadBase
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Nemonico { get; set; }

        // Navigation properties
        public ICollection<UsuarioEmpresa> UsuarioEmpresas { get; set; }
        public ICollection<Rol> Roles { get; set; }
        public ICollection<Menu> Menus { get; set; }
        public ICollection<Carta> Cartas { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
    }
}
