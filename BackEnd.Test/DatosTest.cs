using BackEnd;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public void usuariosGetCreaCopiaDeUsuarios()
    {
        IDatos datos = new Datos();
        datos.GuardarUsuario(new Usuario(-1, "u1", "u1@ubu.es", "DemoLocal26!"));
        Dictionary<int, Usuario> copiaUsuarios1 = datos.Usuarios;
        Dictionary<int, Usuario> copiaUsuarios2 = datos.Usuarios;
        CollectionAssert.AreEquivalent(copiaUsuarios1, copiaUsuarios2);
        Assert.AreNotSame(copiaUsuarios1, copiaUsuarios2);
    }

    [TestMethod]
    public void usuariosGuardaUsuariosConIDExixtente()
    {
        IDatos datos = new Datos();
        datos.GuardarUsuario(new Usuario(-1, "u1", "u1@ubu.es", "DemoLocal26!"));
        datos.GuardarUsuario(new Usuario(1, "u1", "cambio@ubu.es", "DemoLocal26!"));
        Usuario? copiaUsuario = datos.LeerUsuario(1);
        Assert.AreEqual("cambio@ubu.es", copiaUsuario.EMail);
    }

    [TestMethod]
    public void usuariosGuardaUsuariosConIDNoExixtente()
    {
        IDatos datos = new Datos();
        Assert.Throws<KeyNotFoundException> (() => datos.GuardarUsuario(new Usuario(10, "u1", "cambio@ubu.es", "DemoLocal26!")));
    }

    [TestMethod]
    public void usuariosElimina()
    {
        IDatos datos = new Datos();
        datos.Reiniciar();
        Usuario usuario = datos.LeerUsuario(1);
        Assert.AreSame(datos.EliminarUsuario(1),usuario);
    }

    [TestMethod]
    public void usuariosNoExisteElimina()
    {
        IDatos datos = new Datos();
        datos.Reiniciar();
        Assert.IsNull(datos.EliminarUsuario(5000));
    }

    [TestMethod]
    public void leeUsuariosID()
    {
        IDatos datos = new Datos();
        datos.Reiniciar();
        Usuario? copiaUsuario = datos.LeerUsuario(1);
        Assert.AreEqual("uadmin@ubu.es", copiaUsuario.EMail);
    }

    [TestMethod]
    public void leeUsuarioseMail()
    {
        IDatos datos = new Datos();
        datos.GuardarUsuario(new Usuario(-1, "u1", "u6@ubu.es", "DemoLocal26!"));
        Usuario ? copiaUsuario = datos.LeerUsuario("u6@ubu.es");
        Assert.AreEqual("u6@ubu.es", copiaUsuario.EMail);
    }

    [TestMethod]
    public void rolesGetCreaCopiaDeRoles()
    {
        IDatos datos = new Datos();
        datos.GuardarRol(new Rol("Administrador") { IdRol = -1 });
        Dictionary<int, Rol> copiaRol1 = datos.Roles;
        Dictionary<int, Rol> copiaRol2 = datos.Roles;
        CollectionAssert.AreEquivalent(copiaRol1, copiaRol2);
        Assert.AreNotSame(copiaRol1, copiaRol2);
    }

    [TestMethod]
    public void proyectosGetCreaCopiaDeproyectos()
    {
        IDatos datos = new Datos();
        datos.GuardarProyecto(new Proyecto());
        Dictionary<int, Proyecto> copiaPr1 = datos.Proyectos;
        Dictionary<int, Proyecto> copiaPr2 = datos.Proyectos;
        CollectionAssert.AreEquivalent(copiaPr1, copiaPr2);
        Assert.AreNotSame(copiaPr1, copiaPr2);
    }

    [TestMethod]
    public void logsGetCreaCopiaDeLogs()
    {
        IDatos datos = new Datos();
        datos.AgregarLog(new EntradaLog(DateTime.Now, "Información", "mensaje", "usuario@EMail.com"));
        List<EntradaLog> copiaLg1 = datos.Logs;
        List<EntradaLog> copiaLg2 = datos.Logs;
        CollectionAssert.AreEquivalent(copiaLg1, copiaLg2);
        Assert.AreNotSame(copiaLg1, copiaLg2);
    }

}