using BComic.Datos;
using BComic.Models;
using BComic.Models.ViewModels;
using BComic.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BComic.Controllers
{
    [Authorize]
    public class CarroController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ProductoUsuarioVM productoUsuarioVM { get; set; }
        public CarroController(ApplicationDbContext db)
        {
            _db= db;
        }
        public IActionResult Index()
        {
            List<CarroCompra> carroCompraList = new List<CarroCompra>();

            if(HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SesisonCarroCompras)!= null 
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SesisonCarroCompras).Count() > 0)
            {
                carroCompraList = HttpContext.Session.Get<List<CarroCompra>>(WC.SesisonCarroCompras);
            }

            List<int>prodEnCarro = carroCompraList.Select(i=>i.ProductoId).ToList();

            IEnumerable<Producto> prodList = _db.Productos.Where(p => prodEnCarro.Contains(p.Id));

            return View(prodList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction("Resumen");
        }

        public IActionResult Resumen()
        {
            //Trae el usuario conectado
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //

            List<CarroCompra> carroCompraList = new List<CarroCompra>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SesisonCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SesisonCarroCompras).Count() > 0)
            {
                carroCompraList = HttpContext.Session.Get<List<CarroCompra>>(WC.SesisonCarroCompras);
            }

            List<int> prodEnCarro = carroCompraList.Select(i => i.ProductoId).ToList();

            IEnumerable<Producto> prodList = _db.Productos.Where(p => prodEnCarro.Contains(p.Id));

            productoUsuarioVM = new ProductoUsuarioVM()
            {
                UsuarioAplicacion = _db.UsuarioAplicacion.FirstOrDefault(u => u.Id == claim.Value),
                ProductoLista = prodList
            };

            return View(productoUsuarioVM);



        }
        public IActionResult Remover(int Id)
        {
            List<CarroCompra> carroCompraList = new List<CarroCompra>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SesisonCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SesisonCarroCompras).Count() > 0)
            {
                carroCompraList = HttpContext.Session.Get<List<CarroCompra>>(WC.SesisonCarroCompras);
            }

            carroCompraList.Remove(carroCompraList.FirstOrDefault(p=>p.ProductoId==Id));
            HttpContext.Session.Set(WC.SesisonCarroCompras, carroCompraList);  

            return RedirectToAction("Index");
        }
    }
}
