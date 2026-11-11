// Permite definir las pruebas y comprobar sus resultados
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;


// Permite utilizar la clase Usuario
using TPMP_bpg_eca.Nucleo.Modelo;
using TPMP_bpg_eca.Nucleo.Seguridad;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
[ExcludeFromCodeCoverage]
public class UsuarioTest
{
    [TestMethod]
    public void CompruebaPassword_ClaveCorrecta_DevuelveTrue()
    {
        // Crea un usuario con una contraseña conocida
        var usuario = new Usuario(
            1, "borja", "borja@example.com", "ClaveSegur1!");

        // Comprueba la misma contraseña con la que se creó el usuario
        bool resultado = usuario.CompruebaPassword("ClaveSegur1!");

        // La contraseña correcta debe aceptarse
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void CompruebaPassword_ClaveIncorrecta_DevuelveFalse()
    {
        // Crea el usuario con su contraseña original
        var usuario = new Usuario(
            1, "borja", "borja@example.com", "ClaveSegur1!");

        // Prueba una contraseña diferente
        bool resultado = usuario.CompruebaPassword("OtraClave12!");

        // Una contraseña diferente debe rechazarse
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void CompruebaPassword_CambioDeMayuscula_DevuelveFalse()
    {
        // Crea el usuario con la contraseña que empieza por C mayúscula
        var usuario = new Usuario(
            1, "borja", "borja@example.com", "ClaveSegur1!");

        // Cambia únicamente la primera letra a minúscula
        bool resultado = usuario.CompruebaPassword("claveSegur1!");

        // Las contraseñas distinguen entre mayusculas y minusculas
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void CompruebaPassword_EntradaNoValida_DevuelveFalse(
        string? clave)
    {
        // Crea un usuario con una contraseña valida
        var usuario = new Usuario(
            1, "borja", "borja@example.com", "ClaveSegur1!");

        // Prueba cada entrada indicada en los DataRow
        // El signo ! permite pasar null sin el aviso del compilador
        // No cambia el valor que recibe el método
        bool resultado = usuario.CompruebaPassword(clave!);

        // Las entradas nulas, vacias o de espacios deben rechazarse
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void CompruebaPassword_TrasUnFallo_AceptaLaClaveCorrecta()
    {
        // Crea el usuario con su contraseña original
        var usuario = new Usuario(
            1, "borja", "borja@example.com", "ClaveSegur1!");

        // Comprueba que una contraseña incorrecta se rechaza
        Assert.IsFalse(usuario.CompruebaPassword("OtraClave12!"));

        // Comprueba que el intento anterior no ha alterado sus credenciales
        Assert.IsTrue(usuario.CompruebaPassword("ClaveSegur1!"));
    }

    [TestMethod]
    public void CambiarCorreo_ConservaElIdAsignado()
    {
        // Creamos un usuario nuevo, todavía sin un ID definitivo
        var usuario = new Usuario(-1, "borja", "inicial@example.com", "ClaveSegur1!");

        // Comprobamos los datos con los que se ha creado
        Assert.AreEqual(-1, usuario.IdUsuario);
        Assert.AreEqual("inicial@example.com", usuario.EMail);

        // Le asignamos un ID, como hace la aplicación al guardarlo
        usuario.IdUsuario = 7;

        // Más adelante, el usuario cambia su dirección de correo
        usuario.EMail = "nuevo@example.com";

        // Debe tener el correo nuevo y conservar el ID que se le asignó
        Assert.AreEqual("nuevo@example.com", usuario.EMail);
        Assert.AreEqual(7, usuario.IdUsuario);
    }

    [TestMethod]
    public void GeneradoresDeClave_CreanUnaSalNuevaYUnHashVerificable()
    {
        var usuario = new Usuario(1, "borja", "borja@example.com", "ClaveSegur1!");

        // Cada llamada debe generar una sal nueva
        string sal = usuario.generadorSalClave();
        string otraSal = usuario.generadorSalClave();
        Assert.AreNotEqual(sal, otraSal, "Las dos sales deben ser distintas.");

        // El hash debe corresponder a esta contraseña y a la sal que le pasamos
        string hash = usuario.generadorHashClave("ClaveSegur1!", sal);
        Assert.IsTrue(ServicioHash.Verificar("ClaveSegur1!", hash, sal));
        Assert.IsFalse(ServicioHash.Verificar("OtraClave12!", hash, sal));
    }


}