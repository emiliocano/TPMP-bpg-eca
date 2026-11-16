using BackEnd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPMP_bpg_eca.Nucleo.Modelo;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
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

        var usuario = new Usuario(1, "admin", "uadmin@ubu.es", "DemoLocal26!")
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
        Assert.AreEqual("Rol actualizado", datos.LeerRol(1)?.Nombre);
        Assert.AreEqual("Proyecto actualizado", datos.LeerProyecto(1)?.Nombre);
        Assert.AreEqual(2, datos.Usuarios.Count);
        Assert.AreEqual(1, datos.Roles.Count);
        Assert.AreEqual(1, datos.Proyectos.Count);
    }
}