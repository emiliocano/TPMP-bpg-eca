using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TPMP_bpg_eca.Nucleo.Servicios

{
    public static class Formatos
    {
        public static string FormatearFecha(DateTime fecha) {
            if (fecha == default) {
                return "No consta";
            }
            return fecha.ToString("dd/MM/yyyy HH:mm:ss");
            //Propuesta:
            //return fecha.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
