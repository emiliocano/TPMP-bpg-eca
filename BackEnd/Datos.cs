using TPMP_bpg_eca.Nucleo.Modelo;

namespace BackEnd
{
    public class Datos : IDatos
    {
        private readonly Dictionary<int, Usuario> _usuarios;
        private readonly Dictionary<int, Rol> _roles;
        private readonly Dictionary<int, Proyecto> _proyectos;
        private readonly List<EntradaLog> _logs;
        //Crea otro diccionario con las mismas entradas. La copia protege la estructura de la colección
        //Los objetos que contiene son referencias a las entidades almacenadas
        public Dictionary<int, Usuario> Usuarios => new(_usuarios);
        public Dictionary<int, Rol> Roles => new(_roles);
        public Dictionary<int, Proyecto> Proyectos => new(_proyectos);
        public List<EntradaLog> Logs => new(_logs);

        private int SiguienteIdUsuario { get; set; }
        private int SiguienteIdRol{ get; set; }
        private int SiguienteIdProyecto { get; set; }

        public Datos()
        {
            _usuarios = new Dictionary<int, Usuario>();
            _proyectos = new Dictionary<int, Proyecto>();
            _roles = new Dictionary<int, Rol>();
            _logs = new List<EntradaLog>();

           Reiniciar();
        }

        #region _usuarios
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

            Usuario tester1 = new Usuario(
                -1,
                "tester1",
                "tester1@ubu.es",
                "DemoLocal26!");

            GuardarUsuario(tester1);

        }
        public Usuario? GuardarUsuario(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            string mensaje;

            if (usuario.IdUsuario < 0)
            {
                usuario.IdUsuario = SiguienteIdUsuario++;
                _usuarios.Add(usuario.IdUsuario, usuario);
                mensaje = "Usuario añadido";
            }
            else if (_usuarios.ContainsKey(usuario.IdUsuario))
            {
                _usuarios[usuario.IdUsuario] = usuario;
                mensaje = "Usuario actualizado";
            }
            else
            {
                throw new KeyNotFoundException(
                    $"No se encontró el usuario con Id {usuario.IdUsuario}");
            }

            AgregarLog(new EntradaLog(
                DateTime.Now, "Información", mensaje, usuario.EMail));

            return usuario;
        }

        public Usuario? LeerUsuario(int idUsuario)
        {
            // La página principal recupera al usuario usando el ID guardado en sesión
            return _usuarios.GetValueOrDefault(idUsuario);
        }

        public Usuario? LeerUsuario(string eMail)
        {
            // La pantalla de contraseña busca al usuario por su correo
            // Si no hay coincidencia, FirstOrDefault devuelve null
            return _usuarios.Values.FirstOrDefault(usuario =>
                string.Equals(usuario.EMail, eMail, StringComparison.OrdinalIgnoreCase));
        }

        public Usuario? EliminarUsuario(int idUsuario)
        {//Remove elimina la entrada del diccionario y recupera el usuario eliminado
            return _usuarios.Remove(idUsuario, out var usuario) ? usuario : null;
        }
        #endregion _usuarios

        #region _roles
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
            _logs.Add(log);
        }

        public Rol? GuardarRol(Rol rol)
        {
            ArgumentNullException.ThrowIfNull(rol);

            if (rol.IdRol < 0)
            {
                rol.IdRol = SiguienteIdRol++;
                _roles.Add(rol.IdRol, rol);
            }
            else if (_roles.ContainsKey(rol.IdRol))
            {
                _roles[rol.IdRol] = rol;
            }
            else
            {
                throw new KeyNotFoundException(
                    $"No se encontró el rol con Id {rol.IdRol}");
            }

            return rol;
        }


        public List<Proyecto> ProyectosTodos()
        {
            return new List<Proyecto>(_proyectos.Values);
        }


        public Rol? LeerRol(int idRol)
        {
            return _roles.GetValueOrDefault(idRol);
        }

        public Rol? EliminarRol(int idRol)
        {//Remove quita la entrada del diccionario y permite devolver el rol eliminado
            return _roles.Remove(idRol, out var rol) ? rol : null;
        }
        #endregion _roles

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
        {//el método devuelve el objeto eliminado o null si ese ID no existe
            return _proyectos.Remove(idProyecto, out var proyecto) ? proyecto : null;
        }

        public Proyecto? GuardarProyecto(Proyecto proyecto)
        {
            // Comprueba que hemos recibido un proyecto
            ArgumentNullException.ThrowIfNull(proyecto);

            if (proyecto.IdProyecto < 0)
            {
                // Proyecto.cs ya utiliza -1 para un proyecto nuevo
                proyecto.IdProyecto = SiguienteIdProyecto++;
                _proyectos.Add(proyecto.IdProyecto, proyecto);
            }
            else if (_proyectos.ContainsKey(proyecto.IdProyecto))
            {
                // Si el ID ya existe, actualiza ese proyecto
                _proyectos[proyecto.IdProyecto] = proyecto;
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
            return _proyectos.GetValueOrDefault(idProyecto);
        }
        #endregion Proyecto

        public int LimpiarLog()
        {
            int entradasEliminadas = _logs.Count;
            _logs.Clear();
            return entradasEliminadas;
        }
        // TODO: Poner los contadores a cero y crear ususarios, roles y proyectos iniciales para probar
        public void Reiniciar()
        {
            // Vacía los datos que hubiera de una ejecución anterior del reinicio
            _usuarios.Clear();
            _roles.Clear();
            _proyectos.Clear();
            

            // El primer elemento de cada tipo recibirá el identificador 1
            SiguienteIdUsuario = 1;
            SiguienteIdRol = 1;
            SiguienteIdProyecto = 1;

            // Carga los datos de ejemplo
            InicializarRoles();
            InicializarUsuarios();
            InicializarProyectos();
            LimpiarLog();
        }
    }
}
