﻿namespace BComic_Modelos.ViewModels
{
    public class ProductoUsuarioVM
    {

        public ProductoUsuarioVM()
        {
            ProductoLista = new List<Producto>();   
        }

        public UsuarioAplicacion UsuarioAplicacion { get; set; }

        public IList<Producto> ProductoLista { get; set; }
    }
}
