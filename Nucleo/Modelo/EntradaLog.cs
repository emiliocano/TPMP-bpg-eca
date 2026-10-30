using System;

namespace TPMP_bpg_eca.Nucleo.Modelo
{
    // Guarda la información de un suceso de la aplicación
    public class EntradaLog
    {
        // Fecha y hora en las que ocurrió
        public DateTime FechaHora { get; set; }

        // Tipo de entrada, por ejemplo información o error
        public string Nivel { get; set; }

        // Explicación de lo que ha ocurrido
        public string Mensaje { get; set; }

        // Usuario relacionado con el suceso
        public string NombreUsuario { get; set; }

        // Crea la entrada con los datos recibidos
        public EntradaLog(
            DateTime fechaEntrada,
            string nivel,
            string mensaje,
            string usuario)
        {
            // Guarda la fecha recibida
            FechaHora = fechaEntrada;

            // Guarda el nivel del suceso
            Nivel = nivel;

            // Guarda el mensaje
            Mensaje = mensaje;

            // Guarda el nombre del usuario
            NombreUsuario = usuario;
        }
    }
}