using Microsoft.AspNetCore.Identity;

namespace BComic_Modelos
{
    public class UsuarioAplicacion : IdentityUser
    {
        public string NombreCompleto { get; set; }

    }
}
