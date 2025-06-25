using System.ComponentModel.DataAnnotations;

namespace waitly_API.Models.DTOs
{
    public class PermisoDTO
    {
        public int Id { get; set; }
        public string Nemonico { get; set; }
        public string Descripcion { get; set; }
        public int IdTipo { get; set; }
        public string NombreTipo { get; set; }
        public List<string> Pantallas { get; set; } = new List<string>();
    }

    public class CreatePermisoDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public int IdTipo { get; set; }

        public List<int> IdPantallas { get; set; } = new List<int>();
    }

    public class UpdatePermisoDTO
    {
        [StringLength(100, MinimumLength = 3)]
        public string Nemonico { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public int? IdTipo { get; set; }

        public List<int> IdPantallas { get; set; }
    }
}