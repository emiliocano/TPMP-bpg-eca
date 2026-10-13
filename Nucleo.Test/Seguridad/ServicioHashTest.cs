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
        /// Si existe un metodo implementado que devuele algo del tipo
        /// ResultadoHash no saltara la asercion y pasara el test

        ServicioHash servicio_ED_01 = new ServicioHash();
        Assert.IsInstanceOfType<ResultadoHash>(servicio_ED_01.Generar("texto"));

        /// <summary>
        /// ED-02 Parámetro de entrada
        /// El método Generar deberá recibir como parámetro un texto a partir
        /// del cual se generará un hash criptográfico.
        /// <summary>
        /// Comprueba que al pasarle un texto similar a una clave a Generer, 
        /// estedevuelve un string.

        string texto_ED_02 = "Cl4ve_de_prueba";
        ServicioHash servicio_ED_02 = new ServicioHash();
        Assert.IsInstanceOfType<string>(servicio_ED_02.Generar(texto_ED_02));

        /// <summary>
        /// ED-03 Validación de entrada
        /// El método Generar deberá rechazar los valores nulos, vacíos o
        /// compuestos únicamente por espacios en blanco.
        /// <summary>

        /// <summary>
        /// ED-04 Gestión de entradas no válidas
        /// Cuando el método Generar reciba un valor nulo, vacío o compuesto
        /// únicamente por espacios en blanco, deberá lanzar una excepción de
        /// tipo InvalidOperationException.
        /// <summary>

        /// <summary>
        /// ED-05 Resultado de la operación
        /// Cuando la operación finalice correctamente, el método Generar deberá
        /// devolver una instancia de la clase ResultadoHash.
        /// <summary>

        /// <summary>
        /// ED-06 Generación de la sal
        /// Durante cada ejecución deberá generarse una sal independiente para
        /// utilizarla en el cálculo del hash.
        /// <summary>

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
