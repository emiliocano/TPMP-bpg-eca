using System.Diagnostics.CodeAnalysis;
using TPMP_bpg_eca.Nucleo.Seguridad;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
[ExcludeFromCodeCoverage]
public class ServicioHashTest
{
    private const string Clave = "Cl4ve_de_prueba";

    // ED-01, ED-02, ED-05, ED-09, ED-10, ED-11, ED-12 y ED-Extra.
    [TestMethod]
    public void GenerarYVerificar_ConTextoValido_RecuperaUnResultadoCompleto()
    {
        ResultadoHash resultado = ServicioHash.Generar(Clave);

        Assert.IsNotNull(resultado);
        Assert.IsFalse(string.IsNullOrWhiteSpace(resultado.Hash));
        Assert.IsFalse(string.IsNullOrWhiteSpace(resultado.Sal));
        Assert.AreNotEqual(default(DateTime), resultado.Creacion);
        Assert.IsTrue(ServicioHash.Verificar(Clave, resultado.Hash, resultado.Sal));
    }

    // ED-03, ED-04 y ED-Extra.
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("    ")]
    public void Generar_ConTextoInvalido_LanzaInvalidOperationException(string? texto)
    {
        Assert.ThrowsExactly<InvalidOperationException>(() => ServicioHash.Generar(texto!));
    }

    // ED-06, ED-07 y ED-08. Incluye dos generaciones con la misma clave.
    [TestMethod]
    [DataRow(Clave)]
    [DataRow("Otra_cl4ve_de_prueba")]
    public void Generar_EnCadaInvocacion_CreaUnaSalYUnHashDistintos(string segundoTexto)
    {
        var primero = ServicioHash.Generar(Clave);
        var segundo = ServicioHash.Generar(segundoTexto);

        Assert.AreNotEqual(primero.Sal, segundo.Sal);
        Assert.AreNotEqual(primero.Hash, segundo.Hash);
    }

    // ED-13 y ED-17. Las alteraciones conservan el formato Base64.
    [TestMethod]
    public void Verificar_ConTextoHashOSalDistintos_DevuelveFalse()
    {
        var resultado = ServicioHash.Generar(Clave);
        string hashAlterado = CambiarPrimerCaracter(resultado.Hash);
        string salAlterada = CambiarPrimerCaracter(resultado.Sal);

        Assert.IsFalse(ServicioHash.Verificar("Otra_cl4ve_de_prueba", resultado.Hash, resultado.Sal));
        Assert.IsFalse(ServicioHash.Verificar(Clave, hashAlterado, resultado.Sal));
        Assert.IsFalse(ServicioHash.Verificar(Clave, resultado.Hash, salAlterada));
    }

    // ED-14.
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("    ")]
    public void Verificar_ConUnParametroInvalido_DevuelveFalse(string? entrada)
    {
        var resultado = ServicioHash.Generar(Clave);

        Assert.IsFalse(ServicioHash.Verificar(entrada!, resultado.Hash, resultado.Sal));
        Assert.IsFalse(ServicioHash.Verificar(Clave, entrada!, resultado.Sal));
        Assert.IsFalse(ServicioHash.Verificar(Clave, resultado.Hash, entrada!));
    }

    // ED-15 y ED-16. Un dato mal formado debe resultar en false sin propagar excepciones.
    [TestMethod]
    public void Verificar_ConHashOSalDeLongitudIncorrecta_DevuelveFalse()
    {
        var resultado = ServicioHash.Generar(Clave);

        foreach (var hash in VariarLongitud(resultado.Hash))
        {
            Assert.IsFalse(ServicioHash.Verificar(Clave, hash, resultado.Sal),
                $"Se aceptó un hash de {hash.Length} caracteres.");
        }

        foreach (var sal in VariarLongitud(resultado.Sal))
        {
            Assert.IsFalse(ServicioHash.Verificar(Clave, resultado.Hash, sal),
                $"Se aceptó una sal de {sal.Length} caracteres.");
        }
    }

    [TestMethod]
    [DataRow(Clave, true)]
    [DataRow("Otra_cl4ve_de_prueba", false)]
    public void VerificarConResultadoHash_SoloAceptaLaClaveOriginal(
        string claveIntroducida, bool esperado)
    {
        var guardado = ServicioHash.Generar(Clave);

        bool obtenido = ServicioHash.Verificar(claveIntroducida, guardado);

        Assert.AreEqual(esperado, obtenido);
    }

    [TestMethod]
    public void VerificarConResultadoHash_RechazaUnaSalMalFormada()
    {
        var valido = ServicioHash.Generar(Clave);
        var danado = new ResultadoHash { Hash = valido.Hash, Sal = "no-es-base64!" };

        Assert.IsFalse(ServicioHash.Verificar(Clave, danado));
    }

    [TestMethod]
    public void VerificarConResultadoHash_RechazaUnResultadoNulo()
    {
        Assert.IsFalse(ServicioHash.Verificar(Clave, null));
    }

    [TestMethod]
    [DataRow("", "AQID", "BAUG")]
    [DataRow("Clave1!", "", "BAUG")]
    [DataRow("Clave1!", "AQID", "")]
    public void VerificarConResultadoHash_RechazaDatosIncompletos(string texto, string hash, string sal)
    {
        var guardado = new ResultadoHash { Hash = hash, Sal = sal };

        Assert.IsFalse(ServicioHash.Verificar(texto, guardado));
    }

    private static string CambiarPrimerCaracter(string valor)
        => (valor[0] == 'a' ? 'b' : 'a') + valor[1..];

    private static string[] VariarLongitud(string valor)
        => [valor[1..], 'a' + valor, valor[(valor.Length / 2)..], valor + valor];
}
