using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.Entities
{
    public abstract class EntidadBase
    {
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        [StringLength(50)]
        public string UsuarioCreacion { get; set; }

        [StringLength(50)]
        public string UsuarioModificacion { get; set; }

        [StringLength(50)]
        public string UsuarioEliminacion { get; set; }

        [StringLength(255)]
        public string Token { get; set; }

        [StringLength(40)]
        public string IpCreacion { get; set; }

        [StringLength(40)]
        public string IpModificacion { get; set; }

        [StringLength(40)]
        public string IpEliminacion { get; set; }
        public bool Removido { get; set; }
    }
}
