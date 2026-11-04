using BackEnd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TPMP_bpg_eca.Nucleo.Modelo;
using System.ComponentModel.DataAnnotations;
using www.Utilidades;

namespace www.Pages.Login;

public class Index2Model : PageModel
{
    private readonly IDatos _datos;

    public Index2Model(IDatos datos)
    {
        _datos = datos;
    }

    [BindProperty]
    [Required(ErrorMessage = "Debe indicar una contraseña.")]
    [DataType(DataType.Password)]
    public string Contraseña { get; set; } = "";
    public string EMail { get; set; } = "";

    // Tiempo que falta para poder volver a intentar el acceso
    public int SegundosRestantes { get; private set; }
    public IActionResult OnGet()
    {
        EMail = ObtenerEmailPendiente();

        if (string.IsNullOrEmpty(EMail))
        {
            return RedirectToPage("/Login/Index");
        }
        // Recupera el tiempo pendiente al abrir o recargar la página
        ActualizarTiempoRestante();

        return Page();
    }

    public IActionResult OnPostAtras()
    {
        HttpContext.Session.Remove(ClavesSesion.EmailPendiente);

        return RedirectToPage("/Login/Index");
    }

    public IActionResult OnPostAceptar()
    {
        EMail = ObtenerEmailPendiente();

        if (string.IsNullOrEmpty(EMail))
        {
            return RedirectToPage("/Login/Index");
        }

        if (!ModelState.IsValid)
        {
            // Mantiene la cuenta atrás aunque falte la contraseña
            ActualizarTiempoRestante();

            return Page();
        }

        Usuario? usuario = _datos.LeerUsuario(EMail);

        if (usuario is null)
        {
            ModelState.AddModelError(
                "Contraseña",
                "Usuario o contraseña incorrectos.");

            return Page();
        }

        // Comprueba y actualiza los intentos de esta cuenta
        // Evita que dos peticiones simultáneas modifiquen el contador a la vez
        lock (usuario.ControlAcceso)
        {
            // Usa la hora del servidor para comprobar el bloqueo
            DateTimeOffset ahora = DateTimeOffset.UtcNow;

            // Si sigue bloqueado, no permite entrar aunque la contraseña sea correcta
            if (usuario.ControlAcceso.EstaBloqueado(ahora))
            {
                // Redondea hacia arriba para no mostrar cero antes de terminar
                int segundos = (int)Math.Ceiling(
                    usuario.ControlAcceso.TiempoRestante(ahora).TotalSeconds);

                // Entrega este tiempo a la página para mostrar la cuenta atrás
                SegundosRestantes = segundos;

                ModelState.AddModelError(
                    "Contraseña",
                    $"Cuenta bloqueada. Vuelve a intentarlo en {segundos} segundos.");

                return Page();
            }

            // Conserva la comprobación de contraseña
            bool contraseñaCorrecta = usuario.CompruebaPassword(Contraseña);

            if (!contraseñaCorrecta)
            {
                // Registra el fallo cuando termina la comprobación
                DateTimeOffset momentoFallo = DateTimeOffset.UtcNow;
                usuario.ControlAcceso.RegistrarFallo(momentoFallo);

                // El tercer fallo activa el bloqueo de cinco minutos
                if (usuario.ControlAcceso.EstaBloqueado(momentoFallo))
                {
                    // Calcula el tiempo del bloqueo que acaba de comenzar
                    ActualizarTiempoRestante();

                    ModelState.AddModelError(
                        "Contraseña",
                        "Has fallado tres veces. Cuenta bloqueada durante 5 minutos.");
                }
                else
                {
                    // Mantiene el mensaje original mientras no esté bloqueado
                    ModelState.AddModelError(
                        "Contraseña",
                        "Usuario o contraseña incorrectos.");
                }

                return Page();
            }

            // Una contraseña correcta reinicia los fallos anteriores
            usuario.ControlAcceso.RegistrarAcierto(DateTimeOffset.UtcNow);
        }

        // Usuario correctamente autenticado
        HttpContext.Session.Remove(
            ClavesSesion.EmailPendiente);

        HttpContext.Session.SetInt32(
            ClavesSesion.UsuarioId,
            usuario.IdUsuario);

        return RedirectToPage("/Principal/Index");
    }
    private string ObtenerEmailPendiente()
    {
        return HttpContext.Session.GetString(
            ClavesSesion.EmailPendiente) ?? "";
    }
    private void ActualizarTiempoRestante()
    {
        // Si no hay usuario o no está bloqueado, no hay tiempo de espera
        SegundosRestantes = 0;

        Usuario? usuario = _datos.LeerUsuario(EMail);

        if (usuario is null)
        {
            return;
        }

        // Consulta el control de esta cuenta sin modificarlo a la vez
        // que otra petición
        lock (usuario.ControlAcceso)
        {
            // TiempoRestante también comprueba si el bloqueo ha terminado
            TimeSpan restante = usuario.ControlAcceso.TiempoRestante(
                DateTimeOffset.UtcNow);

            // Redondea hacia arriba: una fracción de segundo aún cuenta
            SegundosRestantes = (int)Math.Ceiling(restante.TotalSeconds);
        }
    }

}