using System;
using System.Collections.Generic;
using System.Text;
// Permite usar el servicio que genera y verifica las contraseñas
using TPMP_bpg_eca.Nucleo.Seguridad;

namespace TPMP_bpg_eca.Nucleo.Modelo
{
    /// <summary>
    /// Representa a un usuario dentro del sistema, gestionando sus datos y 
    /// e incorpora funcionalidad para dar acceso al sistema de forma segura.
    /// </summary>
    public class Usuario
    {
        private int      _id;
        private string   _usuario;
        private string   _nombre;
        private string   _apellido1;
        private string   _apellido2;
        private string   _eMail;
        private Rol      _rol;
        private Idioma   _idioma;
        private Boolean  _activo;
        private DateTime _fechaCaducidad;

        private string   _hashClave;
        private string   _salClave;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Usuario"/> con sus credenciales básicas.
        /// </summary>
        /// <param name="id">Identificador único del usuario.</param>
        /// <param name="cuenta">Nombre de usuario o cuenta de acceso al sistema.</param>
        /// <param name="eMail">Dirección de correo electrónico principal del usuario.</param>
        /// <param name="clave">Contraseña en texto plano que será procesada con sal y hash.</param>
        public Usuario(int id, string cuenta, string eMail, string clave)
        {
            // Guarda el identificador del usuario
            _id = id;

            // Guarda el nombre de la cuenta
            _usuario = cuenta;

            // Guarda el correo electrónico
            _eMail = eMail;

            // Genera el hash y su sal a partir de la contraseña
            ResultadoHash resultado = ServicioHash.Generar(clave);

            // Guarda la sal utilizada para generar este hash
            _salClave = resultado.Sal;

            // Guarda el hash para comprobar la contraseña más adelante
            _hashClave = resultado.Hash;
        }

        /// <summary>
        /// Obtiene o establece el identificador único del usuario.
        /// </summary>
        /// <value>El número entero que representa el ID del usuario en el sistema.</value>
        public int IdUsuario { get { return this._id; } set { this._id = value; } }

        /// <summary>
        /// Obtiene o establece el identificador único del usuario.
        /// </summary>
        /// <value>El número entero que representa el ID del usuario en el sistema.</value>
        public string EMail { get { return this._eMail; } set { this._eMail = value; } }

        /// <summary>
        /// Genera una sal criptográfica aleatoria.
        /// </summary>
        /// <returns>Una cadena de texto (string) que contiene la sal generada.</returns>
        public string generadorSalClave()
        {
            return "";
        }

        /// <summary>
        /// Aplica una función hash criptográfica a la clave combinada con su sal correspondiente.
        /// </summary>
        /// <param name="clave">La contraseña en texto plano introducida por el usuario.</param>
        /// <param name="sal">La sal única asociada a este usuario para esta clave.</param>
        /// <returns>La contraseña cifrada en formato de cadena hash.</returns>
        public string generadorHashClave(string clave, string sal)
        {
            return "";
        }


        public bool CompruebaPassword(string Contraseña)
        {
            // Verifica la contraseña usando el hash y la sal guardados
            return ServicioHash.Verificar(Contraseña, _hashClave, _salClave);
        }
    }

    /// <summary>
    /// Representa las preferencias de idioma para un usuario específico.
    /// </summary>
    public class Idioma
    {
        private string _nombreIdioma;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Idioma"/> definiendo el nombre del idioma.
        /// </summary>
        /// <param name="idioma">El nombre del idioma.</param>
        public Idioma(string idioma)
        {
            _nombreIdioma = idioma;
        }

        /// <summary>
        /// Obtiene o establece el nombre del idioma configurado.
        /// </summary>
        /// <value>Cadena de texto con el nombre del idioma.</value>
        public string NombreIdioma { get { return this._nombreIdioma; } set { this._nombreIdioma = value; } }
    }

}
