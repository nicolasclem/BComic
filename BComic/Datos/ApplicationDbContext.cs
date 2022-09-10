using BComic.Models;
using Microsoft.EntityFrameworkCore;

namespace BComic.Datos
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<TipoAplicacion> TipoAplicaciones { get; set; }

        public DbSet <Producto> Productos { get; set; }
    }
}
