﻿using BComic.Datos;
using BComic.Models;
using BComic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BComic.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductoController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Producto> lista = _db.Productos.Include(c => c.Categoria)
                                                       .Include(c => c.TipoAplicacion);
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
                if (productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult  Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (productoVM.Producto.Id == 0)
                {
                    //crear
                    string upload = webRootPath + WC.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productoVM.Producto.ImagenUrl = fileName + extension;
                    _db.Productos.Add(productoVM.Producto);
                }
                else
                {
                   //actulizar
                   var objProducto= _db.Productos.AsNoTracking().FirstOrDefault(p=>p.Id == productoVM.Producto.Id);
                    if(files.Count > 0)//Intenta cargar  una nueva imagen
                    {
                        string upload = webRootPath + WC.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //borrar la imagen  anterior
                        var anteriorFile = Path.Combine(upload,objProducto.ImagenUrl);

                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }
                        //imagen borrada

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productoVM.Producto.ImagenUrl=fileName + extension;

                    }
                    else
                    {
                        productoVM.Producto.ImagenUrl= objProducto.ImagenUrl;
                    }
                    _db.Productos.Update(productoVM.Producto);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");   
            }

            // se llenan nuevamente las listas

            productoVM.CategoriaLista = _db.Categorias.Select(c => new SelectListItem
            {
                Text = c.NombreCategoria,
                Value = c.Id.ToString()
            });
            productoVM.TipoAplicacionLista = _db.TipoAplicaciones.Select(c => new SelectListItem
            {
                Text = c.Nombre,
                Value = c.Id.ToString()
            });
            return View(productoVM);
        }
        
        //get Eliminar
        public IActionResult Eliminar(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            Producto producto = _db.Productos.Include(c=>c.Categoria)
                                             .Include(t => t.TipoAplicacion)
                                             .FirstOrDefault(p=>p.Id==Id);
            if(producto == null)
            {
                return NotFound();
            }
            return View(producto);  
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Producto producto)
        {
            if (producto == null)
            {
                return NotFound();
            }
            //Eliminar la imagen
            string upload = _webHostEnvironment.WebRootPath + WC.ImagenRuta;

            //borrar la imagen  anterior
            var anteriorFile = Path.Combine(upload, producto.ImagenUrl);

            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }
            //fin borrar imagen

            _db.Productos.Remove(producto);
            _db.SaveChanges();
            return RedirectToAction("Index");   
        }
    }
}
