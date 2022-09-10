using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BComic.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre de  producto requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Descripcion Corta de  producto requerida")]
        public string DescripcionCorta { get; set; }
        [Required(ErrorMessage = "Descripcion de  producto requerida")]
        public string DescripcionLarga { get; set; }
        [Required(ErrorMessage = "Descripcion de  producto requerida")]
        [Range(1, double.MaxValue, ErrorMessage ="El precio debe ser mayor a cero")]
        public double Precio{ get; set; }

        public string ImagenUrl { get; set; }


        // foreign Key

        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; }


        public int TipoAplicacionId { get; set; }

        [ForeignKey("TipoAplicacionId")]
        public virtual TipoAplicacion TipoAplicacion { get; set; }

    }
}
