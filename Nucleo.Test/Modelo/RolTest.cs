using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPMP_bpg_eca.Nucleo.Modelo;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
public class RolTest
{
    [TestMethod]
    public void Constructor_ConservaElNombreRecibido()
    {
        // Creamos un rol con el nombre que usa la aplicación
        var rol = new Rol("Administrador");

        // Comprobamos que el constructor ha guardado ese nombre
        Assert.AreEqual("Administrador", rol.Nombre);
    }

    [TestMethod]
    public void CambiarNombre_ActualizaElNombreYConservaElId()
    {
        // Creamos un rol y le asignamos un identificador conocido
        var rol = new Rol("Usuario");
        rol.IdRol = 7;

        // Cambiamos el nombre del mismo rol
        rol.Nombre = "Coordinador";

        // El nombre debe cambiar, pero el identificador debe seguir siendo 7
        Assert.AreEqual("Coordinador", rol.Nombre);
        Assert.AreEqual(7, rol.IdRol);
    }
}