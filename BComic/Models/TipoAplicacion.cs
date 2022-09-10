﻿using System.ComponentModel.DataAnnotations;

namespace BComic.Models
{
    public class TipoAplicacion
    {
        [Key]
        public  int Id { get; set; }
        [Required(ErrorMessage ="El Nombre es un campo obligatorio")]
        public string Nombre { get; set; }
    }
}
