// Permite usar los atributos de pruebas y las comprobaciones Assert
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;


// Permite acceder a la clase ControlIntentosAcceso
using TPMP_bpg_eca.Nucleo.Seguridad;

// Indica el espacio de nombres al que pertenece esta clase
namespace TPMP_bpg_eca.Nucleo.Test;

// Indica a MSTest que esta clase contiene pruebas
[TestClass]
[ExcludeFromCodeCoverage]
public class ControlIntentosAccesoTest
{
    // Guarda una fecha fija como punto de partida de las pruebas
    // Así no dependemos de la hora real ni tenemos que esperar
    private static readonly DateTimeOffset Inicio =
        // Año, mes, día, hora, minutos, segundos y diferencia respecto a UTC
        new DateTimeOffset(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);

    // Indica que el siguiente método es una prueba
    [TestMethod]
    public void DosFallos_NoBloqueanElAcceso()
    {
        // Crea un control nuevo, sin intentos fallidos
        var control = new ControlIntentosAcceso();

        // Registra el primer fallo en la fecha inicial
        control.RegistrarFallo(Inicio);

        // Registra el segundo fallo un segundo después
        control.RegistrarFallo(Inicio.AddSeconds(1));

        // Comprueba que se han registrado dos fallos
        Assert.AreEqual(2, control.IntentosFallidos);

        // Comprueba que dos fallos todavía no bloquean el acceso
        Assert.IsFalse(control.EstaBloqueado(Inicio.AddSeconds(1)));

        // Comprueba que no hay una fecha de fin del bloqueo
        Assert.IsNull(control.BloqueadoHasta);

        // Comprueba que el tiempo de espera es cero
        Assert.AreEqual(
            TimeSpan.Zero, // Tiempo esperado
            control.TiempoRestante(Inicio.AddSeconds(1))); // Tiempo obtenido
    }

    // Indica que el siguiente método es una prueba
    [TestMethod]
    public void TercerFallo_BloqueaDuranteCincoMinutos()
    {
        // Crea un control nuevo para esta prueba
        var control = new ControlIntentosAcceso();

        // Registra el primer fallo
        control.RegistrarFallo(Inicio);

        // Registra el segundo fallo un segundo después
        control.RegistrarFallo(Inicio.AddSeconds(1));

        // Guarda el momento en el que se producirá el tercer fallo
        DateTimeOffset tercerFallo = Inicio.AddSeconds(2);

        // Registra el tercer fallo, que debe activar el bloqueo
        control.RegistrarFallo(tercerFallo);

        // Comprueba que hay tres fallos registrados
        Assert.AreEqual(3, control.IntentosFallidos);

        // Comprueba que el acceso está bloqueado
        Assert.IsTrue(control.EstaBloqueado(tercerFallo));

        // Comprueba que el bloqueo termina cinco minutos después
        Assert.AreEqual(
            // El signo ? permite comparar con una fecha que también admite null
            (DateTimeOffset?)tercerFallo.AddMinutes(5),
            control.BloqueadoHasta); // Fecha de fin guardada por el control

        // Comprueba que al comenzar el bloqueo quedan cinco minutos
        Assert.AreEqual(
            TimeSpan.FromMinutes(5), // Duración esperada.
            control.TiempoRestante(tercerFallo)); // Duración obtenida
    }

    // Indica que el siguiente método es una prueba
    [TestMethod]
    public void AciertoAntesDelBloqueo_ReiniciaLosFallos()
    {
        // Crea un control nuevo para esta prueba
        var control = new ControlIntentosAcceso();

        // Registra el primer fallo
        control.RegistrarFallo(Inicio);

        // Registra el segundo fallo
        control.RegistrarFallo(Inicio.AddSeconds(1));

        // Registra un acceso correcto antes de llegar al tercer fallo
        control.RegistrarAcierto(Inicio.AddSeconds(2));

        // Comprueba que el acceso correcto ha borrado los fallos anteriores
        Assert.AreEqual(0, control.IntentosFallidos);

        // Registra un nuevo fallo después del acceso correcto
        control.RegistrarFallo(Inicio.AddSeconds(3));

        // Comprueba que ahora solo cuenta el nuevo fallo
        Assert.AreEqual(1, control.IntentosFallidos);

        // Comprueba que los fallos anteriores no han provocado un bloqueo
        Assert.IsFalse(control.EstaBloqueado(Inicio.AddSeconds(3)));
    }

    // Indica que el siguiente método es una prueba
    [TestMethod]
    public void IntentosDuranteElBloqueo_NoCambianSuFin()
    {
        // Crea un control nuevo para esta prueba
        var control = new ControlIntentosAcceso();

        // Registra el primer fallo
        control.RegistrarFallo(Inicio);

        // Registra el segundo fallo en el mismo momento
        control.RegistrarFallo(Inicio);

        // Registra el tercer fallo y activa el bloqueo
        control.RegistrarFallo(Inicio);

        // Simula otro fallo cuando ya ha pasado un minuto de bloqueo
        control.RegistrarFallo(Inicio.AddMinutes(1));

        // Simula un acierto cuando han pasado dos minutos de bloqueo
        control.RegistrarAcierto(Inicio.AddMinutes(2));

        // Comprueba que el acierto no ha eliminado el bloqueo
        Assert.IsTrue(control.EstaBloqueado(Inicio.AddMinutes(2)));

        // Comprueba que los intentos durante el bloqueo no cambian el contador
        Assert.AreEqual(3, control.IntentosFallidos);

        // Comprueba que se mantiene la fecha de fin original
        Assert.AreEqual(
            (DateTimeOffset?)Inicio.AddMinutes(5), // Fin esperado.
            control.BloqueadoHasta); // Fin guardado por el control

        // Comprueba que, tras pasar dos minutos, quedan otros tres
        Assert.AreEqual(
            TimeSpan.FromMinutes(3), // Tiempo esperado.
            control.TiempoRestante(Inicio.AddMinutes(2))); // Tiempo obtenido
    }

    // Indica que el siguiente método es una prueba
    [TestMethod]
    public void Bloqueo_SeMantieneHastaJustoAntesDelVencimiento()
    {
        // Crea un control nuevo para esta prueba
        var control = new ControlIntentosAcceso();

        // Registra el primer fallo
        control.RegistrarFallo(Inicio);

        // Registra el segundo fallo
        control.RegistrarFallo(Inicio);

        // Registra el tercer fallo y activa el bloqueo
        control.RegistrarFallo(Inicio);

        // Calcula el momento situado un segundo antes del fin del bloqueo
        DateTimeOffset antesDelFin =
            Inicio.AddMinutes(5).AddSeconds(-1);

        // Comprueba que el acceso sigue bloqueado en ese momento
        Assert.IsTrue(control.EstaBloqueado(antesDelFin));

        // Comprueba que queda exactamente un segundo de espera
        Assert.AreEqual(
            TimeSpan.FromSeconds(1), // Tiempo esperado.
            control.TiempoRestante(antesDelFin)); // Tiempo obtenido
    }

    // Indica que el siguiente método es una prueba
    [TestMethod]
    public void AlCumplirseCincoMinutos_RecuperaTresIntentos()
    {
        // Crea un control nuevo para esta prueba
        var control = new ControlIntentosAcceso();

        // Registra el primer fallo
        control.RegistrarFallo(Inicio);

        // Registra el segundo fallo
        control.RegistrarFallo(Inicio);

        // Registra el tercer fallo y activa el bloqueo
        control.RegistrarFallo(Inicio);

        // Calcula el momento exacto en el que termina el bloqueo
        DateTimeOffset fin = Inicio.AddMinutes(5);

        // Comprueba que el acceso se desbloquea justo al cumplirse el plazo
        Assert.IsFalse(control.EstaBloqueado(fin));

        // Comprueba que el contador de fallos vuelve a cero
        Assert.AreEqual(0, control.IntentosFallidos);

        // Comprueba que se ha borrado la fecha de fin del bloqueo
        Assert.IsNull(control.BloqueadoHasta);

        // Comprueba que ya no queda tiempo de espera
        Assert.AreEqual(TimeSpan.Zero, control.TiempoRestante(fin));

        // Registra el primer fallo después del desbloqueo
        control.RegistrarFallo(fin);

        // Registra el segundo fallo después del desbloqueo
        control.RegistrarFallo(fin.AddSeconds(1));

        // Comprueba que estos dos fallos todavía permiten acceder
        Assert.IsFalse(control.EstaBloqueado(fin.AddSeconds(1)));

        // Registra el tercer fallo después del desbloqueo
        control.RegistrarFallo(fin.AddSeconds(2));

        // Comprueba que el tercer fallo activa un nuevo bloqueo
        Assert.IsTrue(control.EstaBloqueado(fin.AddSeconds(2)));

        // Comprueba que el nuevo bloqueo dura cinco minutos desde ese fallo
        Assert.AreEqual(
            (DateTimeOffset?)fin.AddSeconds(2).AddMinutes(5), // Fin esperado
            control.BloqueadoHasta); // Fin guardado por el control
    }
}