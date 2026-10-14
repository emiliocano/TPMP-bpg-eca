using System.Net;
using TPMP_bpg_eca.Nucleo.Seguridad;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
public class ServicioHashTest
{
    [TestMethod]
    public void Generar_validacion()
    {
        /// <summary>
        /// ED-01 Disponibilidad del método Generar
        /// La clase ServicioHash deberá proporcionar un método público
        /// denominado Generar.
        /// <summary>
        /// No compilara si no esta definida la clave con el metodo
        /// Si existe un metodo implementado que no da errores se pasara la 
        /// asercion sin que salte.

        string ED_01_texto = "Cl4ve_de_prueba";
        ServicioHash = new ServicioHash();
        ED_01_servicio.Generar(ED_01_texto);
        Assert.IsTrue(true);

        /// <summary>
        /// ED-02 Parámetro de entrada
        /// El método Generar deberá recibir como parámetro un texto a partir
        /// del cual se generará un hash criptográfico.
        /// <summary>
        /// Comprueba que al pasarle un texto similar a una clave a Generar, 
        /// este devuelve un valor no nulo

        string ED_02_texto = "Cl4ve_de_prueba";
        ServicioHash ED_02_servicio = new ServicioHash();
        Assert.IsNotNull<>(servicio_ED_02.Generar(ED_02_texto));

        /// <summary>
        /// ED-03 Validación de entrada
        /// El método Generar deberá rechazar los valores nulos, vacíos o
        /// compuestos únicamente por espacios en blanco.
        /// <summary>
        /// Comprueba que se lanza una excpcion para rechazar los valores 
        /// invalidos
        /// 
        string? ED_03_textoNulo = null;
        string ED_03_textoVacio = "";
        string ED_03_textoEnBlanco = "   ";
        ServicioHash ED_03_servicio = new ServicioHash();
        Assert.ThrowsException<Exception>(() => servicio_ED_03.Generar(ED_03_textoNulo));
        Assert.ThrowsException<Exception>(() => servicio_ED_03.Generar(ED_03_textoVacio));
        Assert.ThrowsException<Exception>(() => servicio_ED_03.Generar(ED_03_textoEnBlanco));

        /// <summary>
        /// ED-04 Gestión de entradas no válidas
        /// Cuando el método Generar reciba un valor nulo, vacío o compuesto
        /// únicamente por espacios en blanco, deberá lanzar una excepción de
        /// tipo InvalidOperationException.
        /// <summary>
        /// Comprueba por separado los tres escenarios con generar. Si alguno 
        /// no lanza una excepcion InvalidOperationException salta la asercion 
        /// correspondiente

        string? ED_04_textoNulo = null;
        string ED_04_textoVacio = "";
        string ED_04_textoEnBlanco = "   ";
        ServicioHash ED_04_servicio = new ServicioHash();
        Assert.ThrowsException<InvalidOperationException>(() => servicio_ED_04.Generar(ED_04_textoNulo));
        Assert.ThrowsException<InvalidOperationException>(() => servicio_ED_04.Generar(ED_04_textoVacio));
        Assert.ThrowsException<InvalidOperationException>(() => servicio_ED_04.Generar(ED_04_textoEnBlanco));

        /// <summary>
        /// ED-05 Resultado de la operación
        /// Cuando la operación finalice correctamente, el método Generar deberá
        /// devolver una instancia de la clase ResultadoHash.
        /// <summary>
        /// Comprueba que el valor devuelto por Generar es de tipo ResultadoHash
        /// 

        string ED_05_texto = "Cl4ve_de_prueba";
        ServicioHash ED_05_servicio = new ServicioHash();
        Assert.IsInstanceOfType<ResultadoHash>(servicio_ED_05.Generar(ED_05_texto));

        /// <summary>
        /// ED-06 Generación de la sal
        /// Durante cada ejecución deberá generarse una sal independiente para
        /// utilizarla en el cálculo del hash.
        /// <summary>
        string ED_06_texto = "Cl4ve_de_prueba";
        string? ED_06_sal1 = null;
        string? ED_06_sal2 = null;
        ServicioHash ED_06_servicio = new ServicioHash();
        ResultadoHash ED_06_resultado = new ResultadoHash();
        servicio_ED_06.Generar(ED_06_texto);
        ED_06_sal1 = ResultadoHash.Sal.get();
        servicio_ED_06.Generar(ED_06_texto);
        ED_06_sal2 = ResultadoHash.Sal.get();
        Assert.AreNotEqual(ED_06_sal1, ED_06_sal1);


        /// <summary>
        /// ED-07 Unicidad de los resultados
        /// Dos invocaciones independientes del método Generar realizadas con
        /// el mismo texto deberán producir sales diferentes.
        /// <summary>

        /// <summary>
        /// ED-08 Variabilidad de los hashes
        /// Como consecuencia del uso de sales diferentes, dos invocaciones
        /// realizadas con el mismo texto deberán producir hashes diferentes.
        /// <summary>

        /// <summary>
        /// ED-09 Integridad del resultado
        /// La instancia devuelta deberá contener valores válidos para todas
        /// sus propiedades.
        /// <summary>

    }

    [TestMethod]
    public void Verificar_validacion()
    {
        /// <summary>
        /// ED-10 Disponibilidad del método
        /// La clase ServicioHash deberá proporcionar un método público
        /// denominado Verificar.
        /// <summary>

        /// <summary>
        /// ED-11 Parámetro de entrada
        /// El método deberá recibir: el texto a comprobar, el hash almacenado
        /// y la sal utilizada durante la generación del hash.
        /// <summary>

        /// <summary>
        /// ED-12 Verificación correcta
        /// El método deberá devolver true cuando el texto recibido corresponda
        /// al texto utilizado para generar el hash indicado y se proporcione
        /// la sal asociada.
        /// <summary>

        /// <summary>
        /// ED-13 Verificación incorrecta
        /// El método deberá devolver false cuando el texto recibido no
        /// corresponda al utilizado durante la generación del hash.
        /// <summary>

        /// <summary>
        /// ED-14  Validación de parámetros
        /// El método deberá devolver false cuando cualquiera de los parámetros
        /// recibidos sea nulo, vacío o contenga únicamente espacios en blanco.
        /// <summary>

        /// <summary>
        /// ED-15 Gestión de datos inválidos
        /// El método deberá gestionar de forma controlada los datos de entrada
        /// con formato incorrecto.
        /// <summary>

        /// <summary>
        /// ED-16 Comportamiento ante formatos incorrectos
        /// Si el hash o la sal almacenados presentan un formato no válido,
        /// el método deberá devolver false.
        /// <summary>

        /// <summary>
        /// ED-17 Robustez
        /// El método no deberá propagar excepciones provocadas por datos
        /// almacenados incorrectos.
        /// <summary>

    }
}
