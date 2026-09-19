using System;
using System.Collections.Generic;
using System.Text;

namespace Nucleo.Modelo
{
    public class Usuario
    {
        private int      _id;
        private string   _usuario;
        private string   _nombre;
        private string   _apellido1;
        private string   _apellido2;
        private string   _eMail;
        private Roll     _roll;
        private Idioma   _idioma;
        private Boolean  _activo;
        private DateTime _fechaCaducidad;

        private ulong _hashClave;
        private ulong _salClave;



        
        public Usuario(int id, string cuenta, string eMail, string clave)
        {
            _id = id;
            _usuario = cuenta;
            _eMail = eMail;
            _salClave = generadorSalClave();
            _hashClave = generadorHashClave(clave, _salClave);
        }

        public int Id { get { return this._id; } set { this._id = value; } }

        public ulong generadorSalClave()
        {
            return 1;
        }
        public ulong generadorHashClave(string clave, ulong sal)
        {
            return 1;
        }
    }

    public class Roll
    {
        private string _nombre;

        public Roll(string nombre)
        {
            _nombre = nombre;
        }
        public string Nombre { get { return this._nombre; } set { this._nombre = value; } }
    }

    public class Idioma
    {
        private string _nombreIdioma;

        public Idioma(string idioma)
        {
            _nombreIdioma = idioma;
        }

        public string NombreIdioma { get { return this._nombreIdioma; } set { this._nombreIdioma = value; } }
    }

}
