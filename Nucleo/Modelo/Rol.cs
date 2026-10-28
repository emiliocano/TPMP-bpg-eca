using System;
using System.Collections.Generic;
using System.Text;

namespace TPMP_bpg_eca.Nucleo.Modelo
{
    /// <summary>
    /// Representa el rol del usuario en la aplicación.
    /// </summary>
    public class Rol
    {
        private string _nombre;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Rol"/> especificando su nombre.
        /// </summary>
        /// <param name="nombre">El nombre identificativo del rol (ej. "Administrador", "Usuario").</param>
        public Rol(string nombre)
        {
            _nombre = nombre;
        }

        /// <summary>
        /// Obtiene o establece el nombre del rol.
        /// </summary>
        /// <value>Cadena de texto con el nombre descriptivo del rol.</value>
        public string Nombre { get { return this._nombre; } set { this._nombre = value; } }
    }
}
