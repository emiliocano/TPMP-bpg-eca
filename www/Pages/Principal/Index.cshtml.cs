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

    public string NombreProyectoActual { get; private set; } = SesionAplicacion.SinProyecto;

    public IndexModel(IDatos datos)
    {
        _datos = datos;
    }

    public IActionResult OnGet()
    {
        int? idUsuario = HttpContext.Session.GetInt32(ClavesSesion.UsuarioId);

        if (idUsuario is null || idUsuario <= 0)
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login/Index");
        }

        UsuarioActual = _datos.LeerUsuario(idUsuario.Value);

        if (UsuarioActual is null)
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login/Index");
        }

        NombreUsuario = UsuarioActual.EMail;
        NombreProyectoActual = HttpContext.Session.LeerProyectoActual(_datos)?.Nombre
            ?? SesionAplicacion.SinProyecto;

        return Page();
    }

    public IActionResult OnPostCerrarSesion()
    {
        int? idUsuario = HttpContext.Session.GetInt32(ClavesSesion.UsuarioId);
        if (idUsuario is not null)
        {
            var usuario = _datos.LeerUsuario(idUsuario.Value);
            if (usuario is not null)
            {
                _datos.AgregarLog(new EntradaLog(DateTime.Now, "Información", "Cierre de sesión", usuario.EMail));
            }
        }
        HttpContext.Session.Clear();
        return RedirectToPage("/Login/Index");
    }
}

