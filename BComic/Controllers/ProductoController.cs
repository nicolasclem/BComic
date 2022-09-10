using BComic.Datos;
using BComic.Models;
using BComic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BComic.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductoController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Producto> lista = _db.Productos.Include(c=>c.Categoria)
                                                       .Include(c=>c.TipoAplicacion);
            return View(lista);
        }
        public IActionResult Upsert(int? Id)

        {
            //IEnumerable<SelectListItem> categoriaDropDown = _db.Categorias.Select(c => new SelectListItem
            //{
            //    Text = c.NombreCategoria,
            //    Value = c.Id.ToString()
            //});
            //ViewBag.categoriaDropDown = categoriaDropDown;
            //Producto producto = new Producto();
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _db.Categorias.Select(c => new SelectListItem
                {
                    Text = c.NombreCategoria,
                    Value = c.Id.ToString()
                }),
                TipoAplicacionLista = _db.TipoAplicaciones.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                })
            };

            if (Id == null)
            {
                // crear un nuevo producto
                return View(productoVM);
            }
            else
            {
                productoVM.Producto = _db.Productos.Find(Id);
                if(productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }    
        }
    }
}
