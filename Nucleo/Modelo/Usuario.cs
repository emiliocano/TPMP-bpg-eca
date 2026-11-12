using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
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

        // Guarda los fallos y el tiempo de bloqueo de este usuario
        public ControlIntentosAcceso ControlAcceso { get; } =
            new ControlIntentosAcceso();

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
        /// Obtiene o establece el nombre del usuario.
        /// </summary>
        /// <value>Cadena de texto con el nombre del usuario.</value>
        public string Nombre { get { return this._nombre; } set { this._nombre = value; } }

        /// <summary>
        /// Obtiene o establece el primer apellido del usuario.
        /// </summary>
        /// <value>Cadena de texto con el segundo apellido del usuario.</value>
        public string PrimerApellidoUsuario { get { return this._apellido1; } set { this._apellido1 = value; } }

        /// <summary>
        /// Obtiene o establece el segundo apellido del usuario.
        /// </summary>
        /// <value>Cadena de texto con el segundo appellido del usuario.</value>
        public string SegundoApellidoUsuario { get { return this._apellido2; } set { this._apellido2 = value; } }

        /// <summary>
        /// Obtiene el nombre completo del usuario.
        /// </summary>
        /// <value>Cadena de texto con el nombre completo del usuario.</value>
        public string NombreCompleto() { return this._nombre + " " + this._apellido1 + " " + this._apellido2; }

        /// <summary>
        /// Obtiene o establece los paellidos del usuario.
        /// </summary>
        /// <value>Cadena de texto con los paellidos del usuario.</value>
        public string Apellidos { 
            get { return this._apellido1 + " " + this._apellido2; } 
            set
            {
                string[] ap = value.Split(' ');
                PrimerApellidoUsuario = ap[0];
                SegundoApellidoUsuario = ap.Length > 1 ? ap[1] : "";
            } 
        }

        /// <summary>
        /// Obtiene o establece el e-mail único del usuario.
        /// </summary>
        /// <value>El número entero que representa el e-mail del usuario en el sistema.</value>
        public string EMail { get { return this._eMail; } set { this._eMail = value; } }

        /// <summary>
        /// Genera una sal criptográfica aleatoria.
        /// </summary>
        /// <returns>Una cadena de texto (string) que contiene la sal generada.</returns>
        public string generadorSalClave()
        {
            // Guardamos la sal como texto en el mismo formato que usa ServicioHash
            return Convert.ToBase64String(ServicioHash.CrearSal());
        }

        /// <summary>
        /// Aplica una función hash criptográfica a la clave combinada con su sal correspondiente.
        /// </summary>
        /// <param name="clave">La contraseña en texto plano introducida por el usuario.</param>
        /// <param name="sal">La sal única asociada a este usuario para esta clave.</param>
        /// <returns>La contraseña cifrada en formato de cadena hash.</returns>
        public string generadorHashClave(string clave, string sal)
        {
            // Recuperamos los bytes de la sal recibida y calculamos el hash con ella.
            byte[] bytesSal = Convert.FromBase64String(sal);
            byte[] hash = ServicioHash.Calcular(clave, bytesSal);
            return Convert.ToBase64String(hash);
        }


        public bool CompruebaPassword(string Contraseña)
        {
            // Verifica la contraseña usando el hash y la sal guardados
            return ServicioHash.Verificar(Contraseña, _hashClave, _salClave);
        }

//TODO: implementar
        public bool CambiaPassword(string PasswordActual, string PasswordNueva)
        {
            //comprobar que la clave antigua es correcta
            if (CompruebaPassword(PasswordActual)) {
                //pedir salt
                string s = generadorSalClave();
                //pedir hash
                string h = generadorHashClave(PasswordNueva, s);
                //guardar la salt y el has 
//TODO: Guardar en Datos y en Usuario la sal y la clave                
            }
            return true;
        }

//TODO: implementar
        public bool TieneCredencialesConfiguradas { get { return true; } }

//TODO: implementar
        public bool Activa { get { return true; } }

//TODO: implementar
        public int IntentosErroneos { get { return 1; } }

//TODO: implementar
        public int ListaPasswordsCount { get { return 1; } }

//TODO: implementar
        public DateTime FechaUltimoAcceso { get { return DateTime.Today; } }

//TODO: implementar
        public DateTime UltimoCambioPassword { get { return DateTime.Today; } }

//TODO: implementar
        public DateTime BloquearAccesoHasta { get { return DateTime.Today; } }

//TODO: implementar
        public DateTime CaducidadCuenta { get { return DateTime.Today; } }



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
