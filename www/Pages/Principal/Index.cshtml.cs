using BackEnd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TPMP_bpg_eca.Nucleo.Modelo;
using www.Utilidades;

namespace www.Pages.Principal;

public class IndexModel : PageModel
{
    private readonly IDatos _datos;

    public Usuario? UsuarioActual { get; private set; }

    public string NombreUsuario { get; set; } = "";

    public IndexModel(IDatos datos)
    {
        _datos = datos;
    }

    public IActionResult OnGet()
    {
        int? idUsuario = HttpContext.Session.GetInt32(ClavesSesion.UsuarioId);

        // Si no hay sesión, tampoco hay un ID de usuario
        if (idUsuario is null || idUsuario <= 0)
        {
            EliminarDatosSesion();
            return RedirectToPage("/Login/Index");
        }

        UsuarioActual = _datos.LeerUsuario(idUsuario.Value);

        if (UsuarioActual is null)
        {
            EliminarDatosSesion();
            return RedirectToPage("/Login/Index");
        }

        NombreUsuario = UsuarioActual.EMail; // Sustituir por tu propiedad real

        return Page();
    }

    public IActionResult OnPostCerrarSesion() {
        int? idUsuario = HttpContext.Session.GetInt32(ClavesSesion.UsuarioId);
        if (idUsuario is not null)
        {
            var usuario = _datos.LeerUsuario(idUsuario.Value);
            if (usuario is not null)
            { // Registra quién ha cerrado la sesión antes de borrar sus datos
                _datos.AgregarLog(new EntradaLog(DateTime.Now,"Información","Cierre de sesión",usuario.EMail));
            }
        }
        EliminarDatosSesion();
        return RedirectToPage("/Login/Index");
    }

    private void EliminarDatosSesion() { 
        HttpContext.Session.Remove(ClavesSesion.EmailPendiente);
        HttpContext.Session.Remove(ClavesSesion.UsuarioId);
        HttpContext.Session.Remove(ClavesSesion.ProyectoActualId);
    }
}

