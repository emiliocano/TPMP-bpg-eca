using System.Diagnostics.CodeAnalysis;
using TPMP_bpg_eca.Nucleo.Servicios;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
[ExcludeFromCodeCoverage]
public class FormatosTest
{
    [TestMethod]
    public void FormatoFechaCorrecto()
    {
        DateTime fecha = new DateTime(2026, 5, 1, 16, 28, 32);
        string textoFecha = Formatos.FormatearFecha(fecha);
        Assert.AreEqual("01/05/2026 16:28:32", textoFecha);
    }
    [TestMethod]
    public void FormatoFechaDefault()
    {
        DateTime fecha = default;
        string textoFecha = Formatos.FormatearFecha(fecha);
        Assert.AreEqual("No consta", textoFecha);
    }
}
