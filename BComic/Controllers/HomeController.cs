using BComic_AccesoDatos.Datos;
using BComic_Modelos;
using BComic_Modelos.ViewModels;
using BComic_Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BComic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db )
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Productos= _db.Productos.Include(c=> c.Categoria)
                                        .Include(t=> t.TipoAplicacion),
                Categorias=_db.Categorias
            };

            return View(homeVM);
        }

        public IActionResult Detalle(int Id)

        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SesisonCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SesisonCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SesisonCarroCompras);
            }
            DetalleVM detallleVm = new DetalleVM()
            {
                Producto = _db.Productos.Include(c => c.Categoria).Include(t => t.TipoAplicacion)
                                        .Where(p => p.Id == Id).FirstOrDefault(),
                ExisteEnCarro = false
            };
            foreach (var item in carroComprasLista)
            {
                if(item.ProductoId == Id)
                {
                    detallleVm.ExisteEnCarro=true;
                }
            }

            return View(detallleVm);
            
        }

        [HttpPost, ActionName("Detalle")]
        public IActionResult DetallePost(int Id)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if(HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SesisonCarroCompras)!= null
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SesisonCarroCompras).Count()>0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SesisonCarroCompras);
            }
            carroComprasLista.Add(new CarroCompra { ProductoId = Id });
            HttpContext.Session.Set(WC.SesisonCarroCompras, carroComprasLista); 

            return RedirectToAction("Index");
        }


        public IActionResult RemoverDeCarro(int Id)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SesisonCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SesisonCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SesisonCarroCompras);
            }
            var productoARemover = carroComprasLista.SingleOrDefault(x => x.ProductoId == Id);
            if (productoARemover != null)
            {
                carroComprasLista.Remove(productoARemover);
            }

            HttpContext.Session.Set(WC.SesisonCarroCompras, carroComprasLista);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}