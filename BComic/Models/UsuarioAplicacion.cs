using Microsoft.AspNetCore.Identity;

namespace BComic.Models
{
    public class UsuarioAplicacion : IdentityUser
    {
        public string NombreCompleto { get; set; }

    }
}
