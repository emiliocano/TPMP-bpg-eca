using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using TPMP_bpg_eca.Nucleo.Seguridad;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
public class ResultadoHashTest
{
    [TestMethod]
    public void Almacenamiento_del_hash_de_resuladoHash()
    {
        // ED-18 Especificaciones del Almacenamiento del hash
        // La clase ResultadoHash deberá disponer de una propiedad denominada
        // Hash destinada a contener el hash generado.
    }

    [TestMethod]
    public void Almacenamiento_de_la_sal_de_resuladoHash()
    {
        // ED - 19 Almacenamiento de la sal
        // La clase ResultadoHash deberá disponer de una propiedad denominada
        // Sal destinada a contener la sal utilizada durante el proceso de
        // generación.
    }

    [TestMethod]
    public void Consistencia_del_resultado_de_resuladoHash()
    {
        // ED - 20 Consistencia del resultado
        // Toda instancia de ResultadoHash devuelta por el componente deberá
        // contener valores no nulos y no vacíos para las propiedades Hash y Sal..

    }
}
