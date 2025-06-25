using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.DTOs
{
    public class RolDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public List<string> Permisos { get; set; } = new List<string>();
    }

    public class CreateRolDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public int IdEmpresa { get; set; }

        public List<int> IdPermisos { get; set; } = new List<int>();
    }

    public class UpdateRolDTO
    {
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public int? IdEmpresa { get; set; }

        public List<int> IdPermisos { get; set; }
    }
}