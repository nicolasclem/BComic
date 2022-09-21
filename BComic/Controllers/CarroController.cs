using BComic_AccesoDatos.Datos;
using BComic_Modelos;
using BComic_Modelos.ViewModels;
using BComic_Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace BComic.Controllers
{
    [Authorize]
    public class CarroController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductoUsuarioVM productoUsuarioVM { get; set; }
        public CarroController(ApplicationDbContext db , IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _db= db;
            _webHostEnvironment= webHostEnvironment;
            _emailSender = emailSender;
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
                ProductoLista = prodList.ToList()
            };

            return View(productoUsuarioVM);



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Resumen")]
        public async Task<IActionResult> ResumenPost(ProductoUsuarioVM producto)
        {
            var rutaTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString()
                + "PlantillaOrden.html";
            var subject = "Nueva Orden";
            string HtmlBody = "";

            using (StreamReader sr = System.IO.File.OpenText(rutaTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }
            StringBuilder productoListaSB = new StringBuilder(); 
            foreach (var prod in productoUsuarioVM.ProductoLista)
            {
                productoListaSB.Append($"- Nombre: {prod.Nombre} <span style='font-size:14px'> (ID: {prod.Id}) </span> <br />");
            }

            string messageBody = string.Format(HtmlBody,
                                                productoUsuarioVM.UsuarioAplicacion.NombreCompleto,
                                                productoUsuarioVM.UsuarioAplicacion.Email,
                                                productoUsuarioVM.UsuarioAplicacion.PhoneNumber,
                                                productoListaSB.ToString());

            await _emailSender.SendEmailAsync(WC.EmailAdmin,subject,messageBody);

           return  RedirectToAction("Confirmacion");
        }

        public IActionResult Confirmacion()

        {
            HttpContext.Session.Clear();
            return View();
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
