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

    [TestMethod]
    public void MismoHashYMismaSal_DanResultadosIguales()
    {
        // Creamos dos objetos distintos que contienen los mismos datos
        var primero = new ResultadoHash { Hash = "hash-A", Sal = "sal-1" };
        var segundo = new ResultadoHash { Hash = "hash-A", Sal = "sal-1" };

        // Aunque sean objetos distintos, deben considerarse iguales
        Assert.IsTrue(primero == segundo);

        // Dos objetos iguales deben devolver el mismo código para su uso en colecciones
        Assert.AreEqual(primero.GetHashCode(), segundo.GetHashCode());
    }

    [TestMethod]
    [DataRow("hash-B", "sal-1")] // Cambia solo el hash
    [DataRow("hash-A", "sal-2")] // Cambia solo la sal
    public void CambiarHashOSal_DaResultadosDistintos(string hash, string sal)
    {
        // Este es el resultado que usamos como referencia en los dos casos
        var original = new ResultadoHash { Hash = "hash-A", Sal = "sal-1" };

        // Creamos otro resultado cambiando un solo dato cada vez
        var otro = new ResultadoHash { Hash = hash, Sal = sal };

        // Basta con que cambie uno de los dos datos para que sean distintos
        Assert.IsTrue(original != otro);
    }

    [TestMethod]
    public void Equals_ComoObjeto_ComparaHashYSal()
    {
        var resultado = new ResultadoHash { Hash = "hash-A", Sal = "sal-A" };
        object otroIgual = new ResultadoHash { Hash = "hash-A", Sal = "sal-A" };

        // Al declarar otroIgual como object, se prueba Equals(object)
        Assert.IsTrue(resultado.Equals(otroIgual));

        // Una cadena no es un ResultadoHash, aunque contenga el mismo hash
        Assert.IsFalse(resultado.Equals("hash-A"));
    }

    [TestMethod]
    public void Igualdad_ConResultadosNulos_ComparaAmbosLados()
    {
        ResultadoHash? ausente = null;
        ResultadoHash? otroAusente = null;
        var presente = new ResultadoHash { Hash = "hash-A", Sal = "sal-A" };

        // Dos referencias nulas son iguales
        Assert.IsTrue(ausente == otroAusente);

        // Si solo una es nula, el orden de la comparación no cambia el resultado
        Assert.IsFalse(ausente == presente);
        Assert.IsFalse(presente == ausente);
    }

}
