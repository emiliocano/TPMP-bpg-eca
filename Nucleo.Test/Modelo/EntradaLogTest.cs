using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using TPMP_bpg_eca.Nucleo.Modelo;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
[ExcludeFromCodeCoverage]
public class EntradaLogTest
{
    [TestMethod]
    public void Constructor_GuardaLosCuatroDatosDelSuceso()
    {
        // Usamos una fecha fija para que el resultado no dependa del reloj
        var fecha = new DateTime(2026, 11, 5, 10, 30, 0);

        // Creamos una entrada como la que registra la web al cerrar sesión
        var entrada = new EntradaLog(
            fecha,
            "Información",
            "Cierre de sesión",
            "usuario@ejemplo.es");

        // Comprobamos que cada dato se guardó en la propiedad que le corresponde
        Assert.AreEqual(fecha, entrada.FechaHora);
        Assert.AreEqual("Información", entrada.Nivel);
        Assert.AreEqual("Cierre de sesión", entrada.Mensaje);
        Assert.AreEqual("usuario@ejemplo.es", entrada.NombreUsuario);
    }
}