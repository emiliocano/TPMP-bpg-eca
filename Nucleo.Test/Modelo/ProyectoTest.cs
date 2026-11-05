using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPMP_bpg_eca.Nucleo.Modelo;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
public class ProyectoTest
{
    [TestMethod]
    public void ProyectoNuevo_EmpiezaActivoYSinGuardar()
    {
        // Anotamos la hora justo antes de crear el proyecto
        DateTime antes = DateTime.Now;

        // Creamos un proyecto tal como lo haría la aplicación
        var proyecto = new Proyecto();

        // Anotamos la hora justo después de crearlo
        DateTime despues = DateTime.Now;

        // El -1 indica que todavía no se le ha asignado un ID al guardarlo
        Assert.AreEqual(-1, proyecto.IdProyecto);

        // Un proyecto recién creado debe estar activo
        Assert.IsTrue(proyecto.Activo);

        // Hasta que se rellenen, los tres campos de texto deben estar vacíos
        Assert.AreEqual(string.Empty, proyecto.Nombre);
        Assert.AreEqual(string.Empty, proyecto.Siglas);
        Assert.AreEqual(string.Empty, proyecto.Descripcion);

        // La fecha debe corresponder al momento en que creamos este proyecto
        Assert.IsTrue(
            proyecto.FechaCreacion >= antes && proyecto.FechaCreacion <= despues,
            "La fecha de creación no corresponde al momento de crear el proyecto.");
    }
}