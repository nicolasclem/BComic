using BComic_Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BComic_AccesoDatos.Datos
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<TipoAplicacion> TipoAplicaciones { get; set; }

        public DbSet <Producto> Productos { get; set; }

        public DbSet <UsuarioAplicacion> UsuarioAplicacion { get; set; }
    }
}
