using TPMP_bpg_eca.Nucleo.Seguridad;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
public class ServicioContrasenasTest
{
    [TestMethod]
    public void ServicioContrasenas_Validacion()
    {
        Assert.IsFalse(ServicioContrasenas.EsValida(null));
        Assert.IsFalse(ServicioContrasenas.EsValida("   "));
        Assert.IsFalse(ServicioContrasenas.EsValida("P@ssword"));
        Assert.IsFalse(ServicioContrasenas.EsValida("p@ssw0rd"));
        Assert.IsFalse(ServicioContrasenas.EsValida("P@SSW0RD"));
        Assert.IsFalse(ServicioContrasenas.EsValida("Passw0rd"));
        Assert.IsFalse(ServicioContrasenas.EsValida("P@ssword"));
        Assert.IsFalse(ServicioContrasenas.EsValida("P@ssw0r"));
        Assert.IsTrue(ServicioContrasenas.EsValida("P@ssw0rd"));
    }
}
