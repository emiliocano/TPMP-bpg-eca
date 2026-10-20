using System;
using System.Collections.Generic;
using System.Text;

namespace TPMP_bpg_eca.Nucleo.Seguridad;

public class ControlIntentosAcceso
{
    private const int MaximoIntentos = 3;
    private static readonly TimeSpan DuracionBloqueo =
        TimeSpan.FromMinutes(5);

    public int IntentosFallidos { get; private set; }

    public DateTimeOffset? BloqueadoHasta { get; private set; }

    public bool EstaBloqueado(DateTimeOffset ahora)
    {
        ActualizarBloqueo(ahora);
        return BloqueadoHasta.HasValue;
    }

    public TimeSpan TiempoRestante(DateTimeOffset ahora)
    {
        ActualizarBloqueo(ahora);

        if (BloqueadoHasta is DateTimeOffset fin)
        {
            return fin - ahora;
        }

        return TimeSpan.Zero;
    }

    public void RegistrarFallo(DateTimeOffset ahora)
    {
        ActualizarBloqueo(ahora);

        // Los intentos durante el bloqueo no cambian el plazo
        if (BloqueadoHasta.HasValue)
        {
            return;
        }

        IntentosFallidos++;

        if (IntentosFallidos >= MaximoIntentos)
        {
            BloqueadoHasta = ahora + DuracionBloqueo;
        }
    }

    public void RegistrarAcierto(DateTimeOffset ahora)
    {
        ActualizarBloqueo(ahora);

        // Un acierto no elimina un bloqueo vigente
        if (BloqueadoHasta.HasValue)
        {
            return;
        }

        IntentosFallidos = 0;
    }

    private void ActualizarBloqueo(DateTimeOffset ahora)
    {
        if (BloqueadoHasta is DateTimeOffset fin && ahora >= fin)
        {
            BloqueadoHasta = null;
            IntentosFallidos = 0;
        }
    }
}
