using TPMP_bpg_eca.Nucleo.Seguridad;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
public class ServicioContrasenasTest
{
    [TestMethod]
    public void ServicioContrasenas_Validacion()
    {
        // Sin contenido.
        Assert.IsFalse(ServicioContrasenas.EsValida(null!));
        Assert.IsFalse(ServicioContrasenas.EsValida(""));
        Assert.IsFalse(ServicioContrasenas.EsValida("            "));
        // Longitudes de 11, 12 y 13 caracteres.
        Assert.IsFalse(ServicioContrasenas.EsValida("ClaveSegu1!"));
        Assert.IsTrue(ServicioContrasenas.EsValida("ClaveSegur1!"));
        Assert.IsTrue(ServicioContrasenas.EsValida("ClaveSegur12!"));
        // Falta una mayúscula.
        Assert.IsFalse(ServicioContrasenas.EsValida("clavesegur1!"));

        // Falta una minúscula.
        Assert.IsFalse(ServicioContrasenas.EsValida("CLAVESEGUR1!"));

        // Falta un número.
        Assert.IsFalse(ServicioContrasenas.EsValida("ClaveSegura!"));

        // Falta un carácter especial.
        Assert.IsFalse(ServicioContrasenas.EsValida("ClaveSegur12"));
    }
}
