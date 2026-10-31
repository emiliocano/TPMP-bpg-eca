using TPMP_bpg_eca.Nucleo.Modelo;

namespace BackEnd
{
    public class Datos : IDatos
    {
        public Dictionary<int, Usuario> Usuarios { get;}

        public Dictionary<int, Rol> Roles { get; }

        public Dictionary<int, Proyecto> Proyectos { get; }

        public List<EntradaLog> Logs { get; }

        public int SiguienteIdUsuario { get; set; }
        public int SiguienteIdRol{ get; set; }
        public int SiguienteIdProyecto { get; set; }

        public Datos()
        {
            Usuarios = new Dictionary<int, Usuario>();
            Proyectos = new Dictionary<int, Proyecto>();
            Roles = new Dictionary<int, Rol>();
            Logs = new List<EntradaLog>();
// TODO: llamar a reiniciar para poner los contadores a cero y crear ususarios, roles y proyectos iniciales para probar
//            Reiniciar();
        }

        #region Usuarios
// TODO: añadir metodo para inicializar usuarios iniciales?
        public Usuario? GuardarUsuario(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            // TODO: mirar como tiene que ser la comparación. Que id tiene un usuario nuevo
            if (usuario.IdUsuario < 1)
            {
                usuario.IdUsuario = SiguienteIdUsuario++;
                Usuarios.Add(usuario.IdUsuario, usuario);
// TODO: mirar como añadir logs y corregir
//                AgregarLog(new EntradaLog($"Usuario añadido: {usuario.EMail}"));
            }
            else
            {
                if (Usuarios.ContainsKey(usuario.IdUsuario))
                {
                    Usuarios[usuario.IdUsuario] = usuario;
// TODO: mirar como añadir a logs y corregir
//                  AgregarLog(new EntradaLog($"Usuario actualizado: {usuario.EMail}"));
                }
                else
                {
                    throw new KeyNotFoundException($"No se encontó el usuario con Id {usuario.IdUsuario}");
                }
            }
            return usuario;
        }

        public Usuario? LeerUsuario(int idUsuario)
        {
            throw new NotImplementedException();
        }

        public Usuario? LeerUsuario(string eMail)
        {
            throw new NotImplementedException();
        }

        public Usuario? EliminarUsuario(int idUsuario)
        {
            throw new NotImplementedException();
        }
        #endregion Usuarios

        #region Roles
// TODO: añadir metodo para inicializar roles iniciales?
        public void AgregarLog(EntradaLog log)
        {
            throw new NotImplementedException();
        }

        public Rol? GuardarRol(Rol rol)
        {
// TODO: revisar
            ArgumentNullException.ThrowIfNull(rol);

            if (rol.IdRol < 0)
            {
                rol.IdRol = SiguienteIdRol++;
                Roles.Add(rol.IdRol, rol);
// TODO: mirar como añadir a logs y corregir
//                AgregarLog(new EntradaLog($"Rol añadido: {rol.Nombre}"));
            }
            else
            {
                Roles[rol.IdRol] = rol;
// TODO: mirar como añadir a logs y corregir
//               AgregarLog(new EntradaLog($"Rol actualizado: {rol.Nombre}"));
            }
            return rol;
        }

        public Rol? LeerRol(int idRol)
        {
            throw new NotImplementedException();
        }

        public Rol? EliminarRol(int idRol)
        {
            throw new NotImplementedException();
        }
        #endregion Roles

        #region Proyecto
// TODO: añadir metodo para inicializar proyectos iniciales?
        public Proyecto? EliminarProyecto(int idProyecto)
        {
            throw new NotImplementedException();
        }

        public Proyecto? GuardarProyecto(Proyecto proyecto)
        {
            throw new NotImplementedException();
        }

        public Proyecto? LeerProyecto(int idProyecto)
        {
            throw new NotImplementedException();
        }
        #endregion Proyecto

        public int LimpiarLog()
        {
            throw new NotImplementedException();
        }
// TODO: Poner los contadores a cero y crear ususarios, roles y proyectos iniciales para probar
        public void Reiniciar()
        {
            throw new NotImplementedException();
        }
    }
}
