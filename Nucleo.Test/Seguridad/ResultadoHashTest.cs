using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using TPMP_bpg_eca.Nucleo.Seguridad;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
[ExcludeFromCodeCoverage]
public class ResultadoHashTest
{
    [TestMethod]
    public void Almacenamiento_del_hash_de_resuladoHash()
    {
        // Comprueba que se conserva el hash asignado
        string hashEsperado = "aB12cD34eF56";

        ResultadoHash resultado = new ResultadoHash
        {
            Hash = hashEsperado
        };

        Assert.AreEqual(hashEsperado, resultado.Hash);
    }

    [TestMethod]
    public void Almacenamiento_de_la_sal_de_resuladoHash()
    {
        // Comprueba que se conserva la sal asignada
        string salEsperada = "sAl123456789";

        ResultadoHash resultado = new ResultadoHash
        {
            Sal = salEsperada
        };

        Assert.AreEqual(salEsperada, resultado.Sal);
    }

    [TestMethod]
    public void Consistencia_del_resultado_de_resuladoHash()
    {
        // Genera un resultado a partir de un texto válido
        ResultadoHash resultado = ServicioHash.Generar("ClaveSegur1!");

        // Comprueba que el resultado contiene hash y sal
        Assert.IsNotNull(resultado);
        Assert.IsFalse(string.IsNullOrEmpty(resultado.Hash));
        Assert.IsFalse(string.IsNullOrEmpty(resultado.Sal));

    }
}
