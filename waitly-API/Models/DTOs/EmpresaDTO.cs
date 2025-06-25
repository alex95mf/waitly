﻿using System.ComponentModel.DataAnnotations;

namespace iSit_API.Models.DTOs
{
    public class EmpresaDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Nemonico { get; set; }
    }

    public class CreateEmpresaDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Nemonico { get; set; }
    }

    public class UpdateEmpresaDTO
    {
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Nemonico { get; set; }
    }
}
