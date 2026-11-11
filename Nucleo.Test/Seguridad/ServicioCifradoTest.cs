using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using TPMP_bpg_eca.Nucleo.Seguridad;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
[ExcludeFromCodeCoverage]
public class ServicioCifradoTest
{
    [TestMethod]
    public void CifrarYDescifrar_ConLaMismaClave_RecuperaElTextoOriginal()
    {
        // Usamos tildes y una ñ para comprobar que se conservan al recuperar el texto
        const string original = "Mañana habrá reunión.";

        // Generamos una clave que usaremos tanto para cifrar como para descifrar
        string clave = ServicioCifrado.GenerarClave();

        // Ciframos el mensaje y guardamos el resultado
        string cifrado = ServicioCifrado.Cifrar(original, clave);

        // Usamos la misma clave para recuperar el mensaje
        string recuperado = ServicioCifrado.Descifrar(cifrado, clave);

        // El servicio debe devolver algo distinto del texto que recibió al cifrar
        Assert.AreNotEqual(original, cifrado);

        // Al descifrar, el mensaje debe coincidir exactamente con el original
        Assert.AreEqual(original, recuperado);
    }

    [TestMethod]
    [DataRow("")]               // No se ha introducido ninguna clave
    [DataRow("no-es-base64!")]  // El texto no tiene un formato Base64 válido
    [DataRow("AQID")]           // Es Base64 válido, pero contiene solo 3 bytes
    public void Cifrar_ConClaveNoValida_RechazaLaClave(string clave)
    {
        // Usamos un mensaje válido para que el fallo solo pueda deberse a la clave
        var error = Assert.Throws<System.ArgumentException>(
            () => ServicioCifrado.Cifrar("Mensaje de prueba", clave));

        // Comprobamos que el servicio señala la clave como causa del error
        Assert.AreEqual("claveBase64", error.ParamName);
    }

    [TestMethod]
    public void ContenidoCifradoEsNulo_EnCifrar_ArgumentException()
    {
        // si se pasa null debería lanzarse una exceppcion Agument exception
        // Generamos una clave para descifrar
        string clave = ServicioCifrado.GenerarClave();

        // Ciframos el mensaje y guardamos el resultado
        string cifrado = null;
        Assert.Throws<ArgumentException>(() => ServicioCifrado.Cifrar(cifrado, clave));
    }

    [TestMethod]
    public void ContenidoCifradoEsNulo_EnDescifrar_ArgumentException()
    {
        // si se pasa null debería lanzarse una exceppcion Agument exception
        // Generamos una clave para descifrar
        string clave = ServicioCifrado.GenerarClave();

        // Ciframos el mensaje y guardamos el resultado
        string cifrado = null;
        Assert.Throws<ArgumentException>(() => ServicioCifrado.Descifrar(cifrado, clave));
    }

}