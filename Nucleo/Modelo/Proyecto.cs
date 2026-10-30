using System;


namespace TPMP_bpg_eca.Nucleo.Modelo
{
    // Guarda los datos de un proyecto
    public class Proyecto
    {
        // El valor -1 indica que todavía no se ha guardado
        public int IdProyecto { get; set; } = -1;

        // Nombre del proyecto
        public string Nombre { get; set; } = string.Empty;

        // Abreviatura del nombre
        public string Siglas { get; set; } = string.Empty;

        // Descripción del proyecto
        public string Descripcion { get; set; } = string.Empty;

        // Fecha y hora en las que se crea el objeto
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Un proyecto nuevo empieza activo
        public bool Activo { get; set; } = true;
    }
}