// Permite definir las pruebas y comprobar sus resultados
using Microsoft.VisualStudio.TestTools.UnitTesting;

// Permite utilizar la clase Usuario
using TPMP_bpg_eca.Nucleo.Modelo;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
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
}