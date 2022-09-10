using BComic.Datos;
using BComic.Models;
using Microsoft.AspNetCore.Mvc;

namespace BComic.Controllers
{
    public class TipoAplicacionController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TipoAplicacionController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()

        {
            IEnumerable<TipoAplicacion> lista = _db.TipoAplicaciones;
            return View(lista);
        }

        //get
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(TipoAplicacion tipoAplicacion)
        {
            _db.TipoAplicaciones.Add(tipoAplicacion);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }   
       
        //get Editar
        public IActionResult Editar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _db.TipoAplicaciones.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(TipoAplicacion tipoAplicacion)
        {
            if (ModelState.IsValid)
            {
                _db.TipoAplicaciones.Update(tipoAplicacion);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(tipoAplicacion);
        }
        //get Eliminar
        public IActionResult Eliminar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _db.TipoAplicaciones.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar( TipoAplicacion tipoAplicacion)
        {
            if (tipoAplicacion == null)
            {
                return NotFound();
            }

            _db.TipoAplicaciones.Remove(tipoAplicacion);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
