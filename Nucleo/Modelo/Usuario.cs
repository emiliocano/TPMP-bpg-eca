using System;
using System.Collections.Generic;
using System.Text;

namespace Nucleo.Modelo
{
    public class Usuario
    {
        private int id;
        private string cuenta;
        private string eMail;
        
        public Usuario(int id, string cuenta, string eMail)
        {
            this.id = id;
            this.cuenta = cuenta;
            this.eMail = eMail;
        }

        public int Id { get { return this.id; } set { this.id = value; } }
    }
}
