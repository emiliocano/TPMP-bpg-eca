using System.Diagnostics.CodeAnalysis;
using TPMP_bpg_eca.Nucleo.Modelo;

namespace BackEnd.Test;

[TestClass]
[ExcludeFromCodeCoverage]
public class DatosTest
{
    [TestMethod]
    public void LeerPorCodigo_RecuperaLasEntidadesDeEjemplo()
    {
        IDatos datos = new Datos();

        Assert.AreEqual("uadmin@ubu.es", datos.LeerUsuario(1)?.EMail);
        Assert.AreEqual("Administrador", datos.LeerRol(1)?.Nombre);
        Assert.AreEqual("PROY001", datos.LeerProyecto(1)?.Siglas);
    }

    [TestMethod]
    public void ObtenerColecciones_IncluyeTodosLosElementos()
    {
        IDatos datos = new Datos();
        var rol = new Rol("Tester");
        var proyecto = new Proyecto { Nombre = "Proyecto temporal", Siglas = "TEMP" };

        datos.GuardarRol(rol);
        datos.GuardarProyecto(proyecto);

        CollectionAssert.AreEquivalent(new[] { 1, 2 }, datos.Usuarios.Keys.ToArray());
        CollectionAssert.AreEquivalent(new[] { 1, rol.IdRol }, datos.Roles.Keys.ToArray());
        CollectionAssert.AreEquivalent(new[] { 1, proyecto.IdProyecto }, datos.Proyectos.Keys.ToArray());
        CollectionAssert.AreEquivalent(
            new[] { 1, proyecto.IdProyecto },
            datos.ProyectosTodos().Select(p => p.IdProyecto).ToArray());
    }

    [TestMethod]
    public void Guardar_ConIdExistente_ActualizaLaEntidad()
    {
        IDatos datos = new Datos();

        var usuario = new Usuario(1, "admin", "actualizado@example.test", "DemoLocal26!")
        {
            Nombre = "Usuario actualizado"
        };
        var rol = new Rol("Rol actualizado") { IdRol = 1 };
        var proyecto = new Proyecto
        {
            IdProyecto = 1,
            Nombre = "Proyecto actualizado",
            Siglas = "PROY001"
        };

        datos.GuardarUsuario(usuario);
        datos.GuardarRol(rol);
        datos.GuardarProyecto(proyecto);

        Assert.AreEqual("Usuario actualizado", datos.LeerUsuario(1)?.Nombre);
        Assert.AreEqual("actualizado@example.test", datos.LeerUsuario(1)?.EMail);
        Assert.AreEqual("Rol actualizado", datos.LeerRol(1)?.Nombre);
        Assert.AreEqual("Proyecto actualizado", datos.LeerProyecto(1)?.Nombre);
        Assert.HasCount(2, datos.Usuarios);
        Assert.HasCount(1, datos.Roles);
        Assert.HasCount(1, datos.Proyectos);
    }

    [TestMethod]
    public void Eliminar_ConIdExistente_DevuelveLaEntidadYLaRetira()
    {
        IDatos datos = new Datos();
        var usuarioOriginal = datos.LeerUsuario(1);
        Assert.IsNotNull(usuarioOriginal);

        var usuario = datos.EliminarUsuario(1);
        var rol = datos.EliminarRol(1);
        var proyecto = datos.EliminarProyecto(1);

        Assert.AreSame(usuarioOriginal, usuario);
        Assert.AreEqual("uadmin@ubu.es", usuario?.EMail);
        Assert.AreEqual("Administrador", rol?.Nombre);
        Assert.AreEqual("PROY001", proyecto?.Siglas);

        Assert.IsNull(datos.LeerUsuario(1));
        Assert.IsNull(datos.LeerUsuario("uadmin@ubu.es"));
        Assert.IsNull(datos.LeerRol(1));
        Assert.IsNull(datos.LeerProyecto(1));

        Assert.AreEqual("tester1@ubu.es", datos.LeerUsuario(2)?.EMail);
    }

    [TestMethod]
    public void Reiniciar_RestauraLosDatosDeEjemploYVaciaElLog()
    {
        IDatos datos = new Datos();
        var usuario = new Usuario(-1, "temporal", "reinicio@example.test");
        var rol = new Rol("Rol temporal");
        var proyecto = new Proyecto { Nombre = "Proyecto temporal", Siglas = "TEMP" };

        datos.GuardarUsuario(usuario);
        datos.GuardarRol(rol);
        datos.GuardarProyecto(proyecto);
        datos.AgregarLog(new EntradaLog(DateTime.Now, "Prueba", "Entrada temporal", "temporal"));
        datos.EliminarUsuario(1);

        datos.Reiniciar();

        Assert.IsNull(datos.LeerUsuario(usuario.IdUsuario));
        Assert.IsNull(datos.LeerRol(rol.IdRol));
        Assert.IsNull(datos.LeerProyecto(proyecto.IdProyecto));
        Assert.IsEmpty(datos.Logs);

        Assert.AreEqual("uadmin@ubu.es", datos.LeerUsuario(1)?.EMail);
        Assert.AreEqual("tester1@ubu.es", datos.LeerUsuario(2)?.EMail);
        Assert.AreEqual("Administrador", datos.LeerRol(1)?.Nombre);
        Assert.AreEqual("PROY001", datos.LeerProyecto(1)?.Siglas);
    }

    [TestMethod]
    public void Buscar_EntidadInexistente_DevuelveNull()
    {
        IDatos datos = new Datos();
        const int idInexistente = 9999;

        Assert.IsNull(datos.LeerUsuario(idInexistente));
        Assert.IsNull(datos.LeerUsuario("inexistente@example.test"));
        Assert.IsNull(datos.LeerRol(idInexistente));
        Assert.IsNull(datos.LeerProyecto(idInexistente));
    }

    [TestMethod]
    public void AgregarLog_ConEntradaValida_ConservaSusDatos()
    {
        IDatos datos = new Datos();
        var fecha = new DateTime(2026, 1, 15, 12, 0, 0);
        var entrada = new EntradaLog(fecha, "Información", "Entrada de prueba", "tester");

        datos.AgregarLog(entrada);

        var registros = datos.Logs;
        Assert.HasCount(1, registros);

        var registro = registros[0];
        Assert.AreEqual(fecha, registro.FechaHora);
        Assert.AreEqual("Información", registro.Nivel);
        Assert.AreEqual("Entrada de prueba", registro.Mensaje);
        Assert.AreEqual("tester", registro.NombreUsuario);
    }

    [TestMethod]
    public void GuardarUsuario_ConUsuarioNuevo_LoIncorporaALaColeccion()
    {
        IDatos datos = new Datos();
        int cantidadInicial = datos.Usuarios.Count;
        var usuario = new Usuario(-1, "testerNuevo", "alta@example.test")
        {
            Nombre = "Usuario de prueba"
        };

        var guardado = datos.GuardarUsuario(usuario);

        Assert.IsNotNull(guardado);
        Assert.IsGreaterThan(0, guardado.IdUsuario);
        Assert.HasCount(cantidadInicial + 1, datos.Usuarios);

        var recuperado = datos.LeerUsuario(guardado.IdUsuario);
        Assert.AreEqual("Usuario de prueba", recuperado?.Nombre);
        Assert.AreEqual("alta@example.test", recuperado?.EMail);
    }

    [TestMethod]
    public void GuardarRol_ConRolNuevo_LoIncorporaALaColeccion()
    {
        IDatos datos = new Datos();
        int cantidadInicial = datos.Roles.Count;
        var rol = new Rol("Tester");

        var guardado = datos.GuardarRol(rol);

        Assert.IsNotNull(guardado);
        Assert.IsGreaterThan(0, guardado.IdRol);
        Assert.HasCount(cantidadInicial + 1, datos.Roles);
        Assert.AreEqual("Tester", datos.LeerRol(guardado.IdRol)?.Nombre);
    }

    [TestMethod]
    public void GuardarProyecto_ConProyectoNuevo_LoIncorporaALaColeccion()
    {
        IDatos datos = new Datos();
        int cantidadInicial = datos.Proyectos.Count;
        var proyecto = new Proyecto
        {
            Nombre = "Proyecto de prueba",
            Siglas = "PRUEBA",
            Descripcion = "Descripción de prueba"
        };

        var guardado = datos.GuardarProyecto(proyecto);

        Assert.IsNotNull(guardado);
        Assert.IsGreaterThan(0, guardado.IdProyecto);
        Assert.HasCount(cantidadInicial + 1, datos.Proyectos);

        var recuperado = datos.LeerProyecto(guardado.IdProyecto);
        Assert.AreEqual("Proyecto de prueba", recuperado?.Nombre);
        Assert.AreEqual("PRUEBA", recuperado?.Siglas);
        Assert.AreEqual("Descripción de prueba", recuperado?.Descripcion);
    }

    [TestMethod]
    public void GuardarAltas_AsignaIdentificadoresConsecutivosPorTipo()
    {
        IDatos datos = new Datos();
        var usuario1 = new Usuario(-1, "testerUno", "uno@example.test");
        var usuario2 = new Usuario(-2, "testerDos", "dos@example.test");
        var rol1 = new Rol("Tester");
        var rol2 = new Rol("Revisor");
        var proyecto1 = new Proyecto { Nombre = "Proyecto uno", Siglas = "P01" };
        var proyecto2 = new Proyecto { Nombre = "Proyecto dos", Siglas = "P02" };

        datos.GuardarUsuario(usuario1);
        datos.GuardarRol(rol1);
        datos.GuardarProyecto(proyecto1);
        datos.GuardarUsuario(usuario2);
        datos.GuardarRol(rol2);
        datos.GuardarProyecto(proyecto2);

        CollectionAssert.AreEqual(new[] { 3, 4 }, new[] { usuario1.IdUsuario, usuario2.IdUsuario });
        CollectionAssert.AreEqual(new[] { 2, 3 }, new[] { rol1.IdRol, rol2.IdRol });
        CollectionAssert.AreEqual(new[] { 2, 3 }, new[] { proyecto1.IdProyecto, proyecto2.IdProyecto });
    }

    [TestMethod]
    public void Usuarios_DevuelveUnaCopiaDeLaColeccion()
    {
        IDatos datos = new Datos();
        var copia = datos.Usuarios;
        var otraCopia = datos.Usuarios;

        Assert.AreNotSame(copia, otraCopia);
        CollectionAssert.AreEquivalent(otraCopia, copia);

        copia.Clear();
        CollectionAssert.AreEquivalent(otraCopia, datos.Usuarios);
    }

    [TestMethod]
    public void Roles_DevuelveUnaCopiaDeLaColeccion()
    {
        IDatos datos = new Datos();
        var copia = datos.Roles;
        var otraCopia = datos.Roles;

        Assert.AreNotSame(copia, otraCopia);
        CollectionAssert.AreEquivalent(otraCopia, copia);

        copia.Clear();
        CollectionAssert.AreEquivalent(otraCopia, datos.Roles);
    }

    [TestMethod]
    public void Proyectos_DevuelveUnaCopiaDeLaColeccion()
    {
        IDatos datos = new Datos();
        var copia = datos.Proyectos;
        var otraCopia = datos.Proyectos;

        Assert.AreNotSame(copia, otraCopia);
        CollectionAssert.AreEquivalent(otraCopia, copia);

        copia.Clear();
        CollectionAssert.AreEquivalent(otraCopia, datos.Proyectos);
    }

    [TestMethod]
    public void Logs_DevuelveUnaCopiaDeLaColeccion()
    {
        IDatos datos = new Datos();
        var fecha = new DateTime(2026, 1, 15, 12, 0, 0);
        datos.AgregarLog(new EntradaLog(fecha, "Información", "Mensaje de prueba", "tester"));

        var copia = datos.Logs;
        var otraCopia = datos.Logs;

        Assert.AreNotSame(copia, otraCopia);
        CollectionAssert.AreEquivalent(otraCopia, copia);

        copia.Clear();
        CollectionAssert.AreEquivalent(otraCopia, datos.Logs);
    }

    [TestMethod]
    public void Guardar_ConIdInexistente_RechazaLaActualizacion()
    {
        IDatos datos = new Datos();
        const int idInexistente = 5000;
        var usuario = new Usuario(idInexistente, "tester", "cambio@example.test");
        var rol = new Rol("Tester") { IdRol = idInexistente };
        var proyecto = new Proyecto { IdProyecto = idInexistente };

        Assert.ThrowsExactly<KeyNotFoundException>(() => datos.GuardarUsuario(usuario));
        Assert.ThrowsExactly<KeyNotFoundException>(() => datos.GuardarRol(rol));
        Assert.ThrowsExactly<KeyNotFoundException>(() => datos.GuardarProyecto(proyecto));
    }

    [TestMethod]
    public void Eliminar_ConIdInexistente_DevuelveNull()
    {
        IDatos datos = new Datos();
        const int idInexistente = 5000;

        Assert.IsNull(datos.EliminarUsuario(idInexistente));
        Assert.IsNull(datos.EliminarRol(idInexistente));
        Assert.IsNull(datos.EliminarProyecto(idInexistente));
    }

    [TestMethod]
    public void LeerUsuario_PorCorreoDeUnAlta_RecuperaElUsuario()
    {
        IDatos datos = new Datos();
        var usuario = new Usuario(-1, "tester", "alta@example.test");
        datos.GuardarUsuario(usuario);

        var encontrado = datos.LeerUsuario("ALTA@example.test");

        Assert.AreSame(usuario, encontrado);
    }

    [TestMethod]
    public void GuardarYLeerColecciones_EnParalelo_ConservaTodasLasAltas()
    {
        IDatos datos = new Datos();
        const int cantidadAltas = 256;
        int rolesIniciales = datos.Roles.Count;
        int proyectosIniciales = datos.Proyectos.Count;

        Parallel.For(0, cantidadAltas, indice =>
        {
            datos.GuardarRol(new Rol($"Rol {indice}"));
            datos.GuardarProyecto(new Proyecto { Nombre = $"Proyecto {indice}", Siglas = $"P{indice}" });
            _ = datos.Roles;
            _ = datos.Proyectos;
            _ = datos.ProyectosTodos();
        });

        Assert.HasCount(rolesIniciales + cantidadAltas, datos.Roles);
        Assert.HasCount(proyectosIniciales + cantidadAltas, datos.Proyectos);
        Assert.HasCount(proyectosIniciales + cantidadAltas, datos.ProyectosTodos());
    }
}
