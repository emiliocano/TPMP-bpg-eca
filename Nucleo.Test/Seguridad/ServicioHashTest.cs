using System.Net;
using TPMP_bpg_eca.Nucleo.Seguridad;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
public class ServicioHashTest
{
    [TestMethod]
    public void Disponibilidad_de_Generar()
    {
        /// <summary>
        /// ED-01 Disponibilidad del método Generar
        /// La clase ServicioHash deberá proporcionar un método público
        /// denominado Generar.
        /// <summary>
        /// No compilara si no esta definida la clase con el metodo
        /// Si existe un metodo implementado que no da errores se pasara la 
        /// asercion sin que salte.

        string ED_01_texto = "Cl4ve_de_prueba";
        ServicioHash ED_01_servicio  = new ServicioHash();
        ED_01_servicio.Generar(ED_01_texto);
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void Parametro_de_entrada_texto_en_generar()
    {
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
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("    ")]
    public void Validacion_de_entrada_en_generar(string? ED_03_texto)
    {
        /// <summary>
        /// ED-03 Validación de entrada
        /// El método Generar deberá rechazar los valores nulos, vacíos o
        /// compuestos únicamente por espacios en blanco.
        /// <summary>
        /// Comprueba que se lanza una excpcion para rechazar los valores 
        /// invalidos
        /// 
        ServicioHash ED_03_servicio = new ServicioHash();
        Assert.Throws<Exception>(() => ED_03_servicio.Generar(ED_03_texto));
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("    ")]
    public void Gestion_de_entradas_no_validas_en_generar(string? ED_04_texto)
    {
        /// <summary>
        /// ED-04 Gestión de entradas no válidas
        /// Cuando el método Generar reciba un valor nulo, vacío o compuesto
        /// únicamente por espacios en blanco, deberá lanzar una excepción de
        /// tipo InvalidOperationException.
        /// <summary>
        /// Comprueba por separado los tres escenarios con generar. Si alguno 
        /// no lanza una excepcion InvalidOperationException salta la asercion 
        /// correspondiente
        ServicioHash ED_04_servicio = new ServicioHash();
        Assert.Throws<InvalidOperationException>(() => ED_04_servicio.Generar(ED_04_texto));
    }

    [TestMethod]
    public void Resultado_de_la_operacion_generar()
    {
        /// <summary>
        /// ED-05 Resultado de la operación
        /// Cuando la operación finalice correctamente, el método Generar deberá
        /// devolver una instancia de la clase ResultadoHash.
        /// <summary>
        /// Comprueba que el valor devuelto por Generar es de tipo ResultadoHash
        /// Comprueba que se genera un hash no vacio
        /// Comprueba que se genera una sal no vacio
        string ED_05_texto = "Cl4ve_de_prueba";
        ServicioHash ED_05_servicio = new ServicioHash();
        ResultadoHash ED_05_resultado = ED_05_servicio.Generar(ED_05_texto);
        Assert.IsInstanceOfType<ResultadoHash>(ED_05_resultado);
        Assert.IsFalse(ED_05_resultado.Hash.Get().length() == 0);
        Assert.IsFalse(ED_05_resultado.Sal.Get().length() == 0);
    }

    [TestMethod]
    public void Generacion_de_sal_independiente()
    {
        /// <summary>
        /// ED-06 Generación de la sal
        /// Durante cada ejecución deberá generarse una sal independiente para
        /// utilizarla en el cálculo del hash.
        /// <summary>
        /// Se prueva que dos ejecucines con claves distintas genera una sal 
        /// independiente para cada una
        string ED_06_texto1 = "Cl4ve_de_prueba1";
        string ED_06_texto2 = "Cl4ve_de_prueba2";
        string? ED_06_sal1 = null;
        string? ED_06_sal2 = null;
        ServicioHash ED_06_servicio = new ServicioHash();
        ResultadoHash ED_06_resultado1 = ED_06_servicio.Generar(ED_06_texto1);
        ResultadoHash ED_06_resultado2 = ED_06_servicio.Generar(ED_06_texto2);
        ED_06_sal1 = ED_06_resultado1.Sal.get();
        ED_06_sal2 = ED_06_resultado2.Sal.get();
        Assert.AreNotEqual(ED_06_sal1, ED_06_sal2);
    }

    [TestMethod]
    public void Unicidad_de_resultados_sal_nueva_cada_generacion()
    {
        /// <summary>
        /// ED-07 Unicidad de los resultados
        /// Dos invocaciones independientes del método Generar realizadas con
        /// el mismo texto deberán producir sales diferentes.
        /// <summary>
        /// Se genera un hash con su sal dos veces con la misma clave y se 
        /// comprueba que las sales son distintas
        string ED_07_texto = "Cl4ve_de_prueba";
        string? ED_07_sal1 = null;
        string? ED_07_sal2 = null;
        ServicioHash ED_07_servicio = new ServicioHash();
        ResultadoHash ED_07_resultado1 = ED_07_servicio.Generar(ED_07_texto);
        ED_07_sal1 = ED_07_resultado1.Sal.get();
        ResultadoHash ED_07_resultado2 = ED_07_servicio.Generar(ED_07_texto);
        ED_07_sal2 = ED_07_resultado2.Sal.get();
        Assert.AreNotEqual(ED_07_sal1, ED_07_sal2);
    }

    [TestMethod]
    public void Variabilidad_de_los_hashes_de_generar()
    {
        /// <summary>
        /// ED-08 Variabilidad de los hashes
        /// Como consecuencia del uso de sales diferentes, dos invocaciones
        /// realizadas con el mismo texto deberán producir hashes diferentes.
        /// <summary>
        /// Se genera un hash con su sal dos veces con la misma clave y se 
        /// comprueba que los hashes son distintas
        string ED_08_texto = "Cl4ve_de_prueba";
        string? ED_08_hash1 = null;
        string? ED_08_hash2 = null;
        ServicioHash ED_08_servicio = new ServicioHash();
        ResultadoHash ED_08_resultado1 = ED_08_servicio.Generar(ED_08_texto);
        ED_08_hash1 = ED_08_resultado1.Hash.get();
        ResultadoHash ED_08_resultado2 = ED_08_servicio.Generar(ED_08_texto);
        ED_08_hash2 = ED_08_resultado2.Hash.get();
        Assert.AreNotEqual(ED_08_hash1, ED_08_hash2);
    }

    [TestMethod]
    public void Integridad_del_resultado_de_generar()
    {
        /// <summary>
        /// ED-09 Integridad del resultado
        /// La instancia devuelta deberá contener valores válidos para todas
        /// sus propiedades.
        /// <summary>
        /// Se genera la sal y el hash a partir de una clave y se comprueba
        /// que los valores sonvalidos
        string ED_09_texto = "Cl4ve_de_prueba";
        string? ED_09_sal = null;
        string? ED_09_hash = null;
        ServicioHash ED_09_servicio = new ServicioHash();
        ResultadoHash ED_09_resultado = ED_09_servicio.Generar(ED_09_texto);
        ED_09_sal = ED_09_resultado.Sal.get();
        ED_09_hash = ED_09_resultado.Hash.get();
        Assert.IsTrue(ED_09_servicio.Verificar(ED_09_texto,ED_09_hash,ED_09_sal));
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("    ")]
    public void Generar_nunca_devvuelve_valores_nulos(string? ED_EX_texto)
    {
        /// <summary>
        /// ED-Extra El método nunca debe devolver valores nulos
        /// Con claves validas o invalidas el metodo no debe devolver un valor
        /// nulo
        /// <summary>
        /// Se prueba una clave valida y una invalida con cada condicion y se
        /// comprueba que el resultado no es nulo
        string? ED_EX_sal = null;
        string? ED_EX_hash = null;
        ServicioHash ED_EX_servicio = new ServicioHash();
        ResultadoHash ED_EX_resultado = ED_EX_servicio.Generar(ED_EX_texto);
        ED_EX_sal = ED_EX_resultado.Sal.get();
        ED_EX_hash = ED_EX_resultado.Hash.get();
        Assert.IsNotNull(ED_EX_servicio.Verificar(ED_EX_texto, ED_EX_hash, ED_EX_sal));
    }

    // Método verificar

    [TestMethod]
    public void Disponibilidad_del_metodo_verificar()
    {
        /// <summary>
        /// ED-10 Disponibilidad del método
        /// La clase ServicioHash deberá proporcionar un método público
        /// denominado Verificar.
        /// <summary>
        /// No compilara si no esta definida la clase con el método
        /// Si existe un método implementado que no da errores se pasara la 
        /// aserción sin que salte.
        string ED_10_texto = "Cl4ve_de_prueba";
        ServicioHash ED_10_servicio  = new ServicioHash();
        ResultadoHash ED_10_resultado.Generar(ED_10_texto);
        ED_10_servicio.Verificar(ED_10_texto, ED_10_resultado.Hash.Get(), ED_10_resultado.Salt.get());
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void Parametro_de_entrada_de_validacion()
    {
        /// <summary>
        /// ED-11 Parámetro de entrada
        /// El método deberá recibir: el texto a comprobar, el hash almacenado
        /// y la sal utilizada durante la generación del hash.
        /// <summary>
        /// Comprueba que al pasarle texto, hash y sal validos genera un booleano
        string ED_11_texto = "Cl4ve_de_prueba";
        ServicioHash ED_11_servicio = new ServicioHash();
        ResultadoHash ED_11_resultado.Generar(ED_11_texto);
        Assert.IsNotInstanceOfType<Boolean>(ED_11_servicio.Verificar(ED_11_texto, ED_11_resultado.Hash.Get(), ED_11_resultado.Salt.get()));
    }

    [TestMethod]
    public void Verificacion_correcta_con_validacion()
    {
        /// <summary>
        /// ED-12 Verificación correcta
        /// El método deberá devolver true cuando el texto recibido corresponda
        /// al texto utilizado para generar el hash indicado y se proporcione
        /// la sal asociada.
        /// <summary>
        /// Se genera una sal y un hash a partir de un texto y se prueba que
        /// la verificacionde los tres valores es correcta.
        string ED_12_texto = "Cl4ve_de_prueba";
        ServicioHash ED_12_servicio = new ServicioHash();
        ResultadoHash ED_12_resultado.Generar(ED_12_texto);
        Assert.IsTrue(ED_12_servicio.Verificar(ED_12_texto, ED_12_resultado.Hash.Get(), ED_12_resultado.Salt.get()));
    }

    [TestMethod]
    public void Verificacion_incorrecta_con_validacion()
    {
        /// <summary>
        /// ED-13 Verificación incorrecta
        /// El método deberá devolver false cuando el texto recibido no
        /// corresponda al utilizado durante la generación del hash.
        /// <summary>
        /// Se genera una sal y un hash a partir de un texto y se prueba que
        /// la verificacionde los tres valores es correcta y que cambiando cada
        /// uno de los tres valores el resultado es clave falsa
        string ED_13_texto = "Cl4ve_de_prueba";
        string ED_13_texto2 = "Cl4ve_de_prueb4";
        string? ED_13_Sal = null;
        string? ED_13_Hash = null;
        string? ED_13_nuevaSal = null;
        string? ED_13_nuevoHash = null;
        ServicioHash ED_13_servicio = new ServicioHash();
        ResultadoHash ED_13_resultado.Generar(ED_13_texto);
        ED_13_Sal = ED_13_resultado.Sal.get();
        ED_13_Hash = ED_13_resultado.Hash.get();
        ED_13_nuevaSal = (ED_13_Sal[0] == 'a' ? 'b' : 'a') + ED_13_Sal[1..];
        ED_13_nuevoHash = (ED_13_Hash[0] == 'a' ? 'b' : 'a') + ED_13_Hash[1..];
        Assert.IsTrue(ED_13_servicio.Verificar(ED_13_texto, ED_13_Hash, ED_13_Sal));
        Assert.IsFalse(ED_13_servicio.Verificar(ED_13_texto2, ED_13_Hash, ED_13_Sal)); // cambia la clave
        Assert.IsFalse(ED_13_servicio.Verificar(ED_13_texto, ED_13_Hash, ED_13_nuevaSal)); // cambia la salt
        Assert.IsFalse(ED_13_servicio.Verificar(ED_13_texto, ED_13_nuevoHash, ED_13_Sal)); // cambia el hash
    }

    [TestMethod]
    public void Validación_de_parámetros_de_validacion()
    {
        /// <summary>
        /// ED-14  Validación de parámetros
        /// El método deberá devolver false cuando cualquiera de los parámetros
        /// recibidos sea nulo, vacío o contenga únicamente espacios en blanco.
        /// <summary>
    }

    [TestMethod]
    public void Gestion_de_datos_invalidos_en_validacion()
    {
        /// <summary>
        /// ED-15 Gestión de datos inválidos
        /// El método deberá gestionar de forma controlada los datos de entrada
        /// con formato incorrecto.
        /// <summary>
    }

    [TestMethod]
    public void Comportamiento_ante_formatos_incorrectos_en_validacion()
    {
        /// <summary>
        /// ED-16 Comportamiento ante formatos incorrectos
        /// Si el hash o la sal almacenados presentan un formato no válido,
        /// el método deberá devolver false.
        /// <summary>
    }

    [TestMethod]
    public void Robustez_de_validacion()
    {
        /// <summary>
        /// ED-17 Robustez
        /// El método no deberá propagar excepciones provocadas por datos
        /// almacenados incorrectos.
        /// <summary>

    }
}
