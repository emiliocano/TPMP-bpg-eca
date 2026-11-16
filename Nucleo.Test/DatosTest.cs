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
}