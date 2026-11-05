using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPMP_bpg_eca.Nucleo.Modelo;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
public class IdiomaTest
{
    [TestMethod]
    public void NombreIdioma_ConservaElNombreInicialYPermiteCambiarlo()
    {
        // Creamos la preferencia de idioma con su valor inicial
        var idioma = new Idioma("Español");

        // Comprobamos el valor inicial antes de modificarlo
        Assert.AreEqual("Español", idioma.NombreIdioma);

        // Cambiamos la preferencia en el mismo objeto
        idioma.NombreIdioma = "Inglés";

        // Al consultarla de nuevo debe aparecer el idioma actualizado
        Assert.AreEqual("Inglés", idioma.NombreIdioma);
    }
}