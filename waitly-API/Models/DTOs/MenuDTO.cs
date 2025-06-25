using System.Text.Json.Serialization;

namespace waitly_API.Models.DTOs
{
    public class MenuDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Nemonico { get; set; }
        public string Descripcion { get; set; }
        public bool VerificaPermiso { get; set; }
        public int Orden { get; set; }
        public int? IdPadre { get; set; }
        public string Url { get; set; }
        public int IdTipo { get; set; }
        public string TipoNombre { get; set; }
        public int IdEmpresa { get; set; }
        public string EmpresaNombre { get; set; }
        public int? IdPermiso { get; set; }
        public string PermisoNombre { get; set; }
        public List<MenuDTO> SubMenus { get; set; } = new List<MenuDTO>();
    }

    public class CreateMenuDTO
    {
        public string Nombre { get; set; }
        public string Nemonico { get; set; }
        public string Descripcion { get; set; }
        public bool VerificaPermiso { get; set; }
        public int Orden { get; set; }
        public int? IdPadre { get; set; }
        public string Url { get; set; }
        public int IdTipo { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdPermiso { get; set; }
    }

    public class UpdateMenuDTO
    {
        public string Nombre { get; set; }
        public string Nemonico { get; set; }
        public string Descripcion { get; set; }
        public bool? VerificaPermiso { get; set; }
        public int? Orden { get; set; }
        public int? IdPadre { get; set; }
        public string Url { get; set; }
        public int? IdTipo { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdPermiso { get; set; }
    }

    public class MenuTreeDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Nemonico { get; set; }
        public string Url { get; set; }
        public int Orden { get; set; }
        public string Tipo { get; set; }
        public List<MenuTreeDTO> SubMenus { get; set; } = new List<MenuTreeDTO>();
    }

    public class MenuUsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Nemonico { get; set; }
        public string Url { get; set; }
        public int Orden { get; set; }
        public string Tipo { get; set; }
        public List<MenuUsuarioDTO> SubMenus { get; set; } = new List<MenuUsuarioDTO>();
    }
}