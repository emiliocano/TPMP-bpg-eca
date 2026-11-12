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
           Reiniciar();
        }

        #region Usuarios
 // TODO: añadir metodo para inicializar usuarios iniciales?
        private void InicializarUsuarios()
        {
            // constructor recibe: ID, cuenta, correo y contraseña
            // El -1 indica que GuardarUsuario debe asignarle un ID
            Usuario admin = new Usuario(
                -1,
                "admin",
                "uadmin@ubu.es",
                "DemoLocal26!");

            GuardarUsuario(admin);
        }
        public Usuario? GuardarUsuario(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);

// TODO: mirar como tiene que ser la comparación. Que id tiene un usuario nuevo
            if (usuario.IdUsuario < 0)
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
            // La página principal recupera al usuario usando el ID guardado en sesión
            return Usuarios.GetValueOrDefault(idUsuario);
        }

        public Usuario? LeerUsuario(string eMail)
        {
            // La pantalla de contraseña busca al usuario por su correo
            // Si no hay coincidencia, FirstOrDefault devuelve null
            return Usuarios.Values.FirstOrDefault(usuario =>
                string.Equals(usuario.EMail, eMail, StringComparison.OrdinalIgnoreCase));
        }

        public Usuario? EliminarUsuario(int idUsuario)
        {
            throw new NotImplementedException();
        }
        #endregion Usuarios

        #region Roles
        // TODO: añadir metodo para inicializar roles iniciales?
        private void InicializarRoles()
        {
            // Un ID negativo indica a GuardarRol que este rol es nuevo
            Rol administrador = new Rol("Administrador") { IdRol = -1 };

            GuardarRol(administrador);
        }
        public void AgregarLog(EntradaLog log)
        {
            // Guarda el suceso que recibe desde la aplicación
            ArgumentNullException.ThrowIfNull(log);
            Logs.Add(log);
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

//TODO: implementar
        public List<Proyecto> ProyectosTodos()
        {

            return new List<Proyecto>();
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
        private void InicializarProyectos()
        {
            // Crea el proyecto de ejemplo
            Proyecto proyecto = new Proyecto
            {
                Nombre = "Proyecto de ejemplo",
                Siglas = "PROY001",
                Descripcion = "Proyecto generado automáticamente"
            };

            GuardarProyecto(proyecto);
        }
        public Proyecto? EliminarProyecto(int idProyecto)
        {
            throw new NotImplementedException();
        }

        public Proyecto? GuardarProyecto(Proyecto proyecto)
        {
            // Comprueba que hemos recibido un proyecto
            ArgumentNullException.ThrowIfNull(proyecto);

            if (proyecto.IdProyecto < 0)
            {
                // Proyecto.cs ya utiliza -1 para un proyecto nuevo
                proyecto.IdProyecto = SiguienteIdProyecto++;
                Proyectos.Add(proyecto.IdProyecto, proyecto);
            }
            else if (Proyectos.ContainsKey(proyecto.IdProyecto))
            {
                // Si el ID ya existe, actualiza ese proyecto
                Proyectos[proyecto.IdProyecto] = proyecto;
            }
            else
            {
                throw new KeyNotFoundException(
                    $"No se encontró el proyecto con Id {proyecto.IdProyecto}");
            }

            return proyecto;
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
            // Vacía los datos que hubiera de una ejecución anterior del reinicio
            Usuarios.Clear();
            Roles.Clear();
            Proyectos.Clear();
            Logs.Clear();

            // El primer elemento de cada tipo recibirá el identificador 1
            SiguienteIdUsuario = 1;
            SiguienteIdRol = 1;
            SiguienteIdProyecto = 1;

            // Carga los datos de ejemplo
            InicializarRoles();
            InicializarUsuarios();
            InicializarProyectos();
        }
    }
}
