using System.Diagnostics.CodeAnalysis;
using System.Net;
using TPMP_bpg_eca.Nucleo.Seguridad;
using static System.Net.Mime.MediaTypeNames;

namespace TPMP_bpg_eca.Nucleo.Test;

[TestClass]
[ExcludeFromCodeCoverage]
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
//        ServicioHash ED_01_servicio  = new ServicioHash();
//        ED_01_servicio.Generar(ED_01_texto);
        ServicioHash.Generar(ED_01_texto);
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
        ResultadoHash? resultado = null;
//        ServicioHash ED_02_servicio = new ServicioHash();
//        resultado = ED_02_servicio.Generar(ED_02_texto);
        resultado = ServicioHash.Generar(ED_02_texto);
        Assert.IsNotNull(resultado);
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
//        ServicioHash ED_03_servicio = new ServicioHash();
        Assert.Throws<Exception>(() => ServicioHash.Generar(ED_03_texto));
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
//        ServicioHash ED_04_servicio = new ServicioHash();
        Assert.Throws<InvalidOperationException>(() => ServicioHash.Generar(ED_04_texto));
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
        Boolean? resultado = null;
//        ServicioHash ED_05_servicio = new ServicioHash();
        ResultadoHash ED_05_resultado = ServicioHash.Generar(ED_05_texto);
        Assert.IsInstanceOfType<ResultadoHash>(ED_05_resultado);
        resultado = ED_05_resultado.Hash.Length == 0;
        Assert.IsFalse(resultado);
        resultado = ED_05_resultado.Sal.Length == 0;
        Assert.IsFalse(resultado);
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
//        ServicioHash ED_06_servicio = new ServicioHash();
        ResultadoHash ED_06_resultado1 = ServicioHash.Generar(ED_06_texto1);
        ResultadoHash ED_06_resultado2 = ServicioHash.Generar(ED_06_texto2);
        ED_06_sal1 = ED_06_resultado1.Sal;
        ED_06_sal2 = ED_06_resultado2.Sal;
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
//        ServicioHash ED_07_servicio = new ServicioHash();
        ResultadoHash ED_07_resultado1 = ServicioHash.Generar(ED_07_texto);
        ED_07_sal1 = ED_07_resultado1.Sal;
        ResultadoHash ED_07_resultado2 = ServicioHash.Generar(ED_07_texto);
        ED_07_sal2 = ED_07_resultado2.Sal;
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
//        ServicioHash ED_08_servicio = new ServicioHash();
        ResultadoHash ED_08_resultado1 = ServicioHash.Generar(ED_08_texto);
        ED_08_hash1 = ED_08_resultado1.Hash;
        ResultadoHash ED_08_resultado2 = ServicioHash.Generar(ED_08_texto);
        ED_08_hash2 = ED_08_resultado2.Hash;
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
        Boolean? resultado = null;
//        ServicioHash ED_09_servicio = new ServicioHash();
        ResultadoHash ED_09_resultado = ServicioHash.Generar(ED_09_texto);
        ED_09_sal = ED_09_resultado.Sal;
        ED_09_hash = ED_09_resultado.Hash;
        resultado = ServicioHash.Verificar(ED_09_texto, ED_09_hash, ED_09_sal);
        Assert.IsTrue(resultado);
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
        Boolean? resultado = null;
//        ServicioHash ED_EX_servicio = new ServicioHash();
        ResultadoHash ED_EX_resultado = ServicioHash.Generar(ED_EX_texto);
        ED_EX_sal = ED_EX_resultado.Sal;
        ED_EX_hash = ED_EX_resultado.Hash;
        resultado = ServicioHash.Verificar(ED_EX_texto, ED_EX_hash, ED_EX_sal);
        Assert.IsNotNull(resultado);
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
//        ServicioHash ED_10_servicio  = new ServicioHash();
        ResultadoHash ED_10_resultado = ServicioHash.Generar(ED_10_texto);
        ServicioHash.Verificar(ED_10_texto, ED_10_resultado.Hash, ED_10_resultado.Sal);
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
//        ServicioHash ED_11_servicio = new ServicioHash();
        ResultadoHash ED_11_resultado = ServicioHash.Generar(ED_11_texto);
        var resultado = ServicioHash.Verificar(ED_11_texto, ED_11_resultado.Hash, ED_11_resultado.Sal);
        Assert.IsNotInstanceOfType<Boolean>(ServicioHash.Verificar(ED_11_texto, ED_11_resultado.Hash, ED_11_resultado.Sal));
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
        Boolean? resultado = null;
//        ServicioHash ED_12_servicio = new ServicioHash();
        ResultadoHash ED_12_resultado = ServicioHash.Generar(ED_12_texto);
        resultado = ServicioHash.Verificar(ED_12_texto, ED_12_resultado.Hash, ED_12_resultado.Sal);
        Assert.IsTrue(resultado);
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
        Boolean? resultado = null;
//        ServicioHash ED_13_servicio = new ServicioHash();
        ResultadoHash ED_13_resultado = ServicioHash.Generar(ED_13_texto);
        ED_13_Sal = ED_13_resultado.Sal;
        ED_13_Hash = ED_13_resultado.Hash;
        ED_13_nuevaSal = (ED_13_Sal[0] == 'a' ? 'b' : 'a') + ED_13_Sal[1..];
        ED_13_nuevoHash = (ED_13_Hash[0] == 'a' ? 'b' : 'a') + ED_13_Hash[1..];
        resultado = ServicioHash.Verificar(ED_13_texto, ED_13_Hash, ED_13_Sal);
        Assert.IsTrue(resultado);
        resultado = ServicioHash.Verificar(ED_13_texto2, ED_13_Hash, ED_13_Sal);
        Assert.IsFalse(resultado); // cambia la clave
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("    ")]
    public void Validación_de_parámetros_de_validacion(string texto)
    {
        /// <summary>
        /// ED-14  Validación de parámetros
        /// El método deberá devolver false cuando cualquiera de los parámetros
        /// recibidos sea nulo, vacío o contenga únicamente espacios en blanco.
        /// <summary>
        /// A parti de un conjunto clave, hash, sal se pruevan las cadenas
        /// invalidas en las tres posiciones
        string ED_14_texto = "Cl4ve_de_prueba";
        string? ED_14_Sal = null;
        string? ED_14_Hash = null;
        Boolean? resultado = null;
//        ServicioHash ED_14_servicio = new ServicioHash();
        ResultadoHash ED_14_resultado = ServicioHash.Generar(ED_14_texto);
        ED_14_Sal = ED_14_resultado.Sal;
        ED_14_Hash = ED_14_resultado.Hash;
        resultado = ServicioHash.Verificar(texto, ED_14_Hash, ED_14_Sal);
        Assert.IsFalse(resultado);
        resultado = ServicioHash.Verificar(ED_14_texto, texto, ED_14_Sal);
        Assert.IsFalse(resultado);
        resultado = ServicioHash.Verificar(ED_14_texto, ED_14_Hash, texto);
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void Gestion_de_datos_invalidos_en_validacion()
    {
        /// <summary>
        /// ED-15 Gestión de datos inválidos
        /// El método deberá gestionar de forma controlada los datos de entrada
        /// con formato incorrecto.
        /// <summary>
        /// Se prueba que con un formato incorrecto (hash y sal) mas corto o 
        /// mas largo que los generados no se lanzan excepciones ni devuelve 
        /// null
        string ED_15_texto = "Cl4ve_de_prueba";
        string? ED_15_Sal = null;
        string? ED_15_Hash = null;
        string? ED_15_nuevaSal = null;
        string? ED_15_nuevoHash = null;
        Boolean? resultado = null;
//        ServicioHash ED_15_servicio = new ServicioHash();
        ResultadoHash ED_15_resultado = ServicioHash.Generar(ED_15_texto);
        ED_15_Sal = ED_15_resultado.Sal;
        ED_15_Hash = ED_15_resultado.Hash;

        // Hash corto
        ED_15_nuevoHash = ED_15_Hash[1..];
        try
        {
            resultado = ServicioHash.Verificar(ED_15_texto, ED_15_nuevoHash, ED_15_Sal);
            Assert.IsNotNull(resultado);
        }
        catch { Assert.Fail(); }

        // Hash largo
        ED_15_nuevoHash = 'a' + ED_15_Hash;
        try
        {
            resultado = ServicioHash.Verificar(ED_15_texto, ED_15_nuevoHash, ED_15_Sal);
            Assert.IsNotNull(resultado);
        }
        catch { Assert.Fail(); }

        // Hash mucho mas corto
        ED_15_nuevoHash = ED_15_Hash[(ED_15_Hash.Length / 2)..];
        try
        {
            resultado = ServicioHash.Verificar(ED_15_texto, ED_15_nuevoHash, ED_15_Sal);
            Assert.IsNotNull(resultado);
        }
        catch { Assert.Fail(); }

        // Hash mucho mas largo
        ED_15_nuevoHash = ED_15_Hash + ED_15_Hash;
        try
        {
            resultado = ServicioHash.Verificar(ED_15_texto, ED_15_nuevoHash, ED_15_Sal);
            Assert.IsNotNull(resultado);
        }
        catch { Assert.Fail(); }

        // Sal Corta
        ED_15_nuevaSal = ED_15_Sal[1..];
        try
        {
            resultado = ServicioHash.Verificar(ED_15_texto, ED_15_Hash, ED_15_nuevaSal);
            Assert.IsNotNull(resultado);
        }
        catch { Assert.Fail(); }

        // Sal larga
        ED_15_nuevaSal = 'a' + ED_15_Sal;
        try
        {
            resultado = ServicioHash.Verificar(ED_15_texto, ED_15_Hash, ED_15_nuevaSal);
            Assert.IsNotNull(resultado);
        }
        catch { Assert.Fail(); }

        // Sal mucho mas corta
        ED_15_nuevaSal = ED_15_Sal[(ED_15_Sal.Length / 2)..];
        try
        {
            resultado = ServicioHash.Verificar(ED_15_texto, ED_15_Hash, ED_15_nuevaSal);
            Assert.IsNotNull(resultado);
        }
        catch { Assert.Fail(); }

        // Sal mucho mas larga
        ED_15_nuevaSal = ED_15_Sal + ED_15_Sal;
        try
        {
            resultado = ServicioHash.Verificar(ED_15_texto, ED_15_Hash, ED_15_nuevaSal);
            Assert.IsNotNull(resultado);
        }
        catch { Assert.Fail(); }
    }

    [TestMethod]
    public void Comportamiento_ante_formatos_incorrectos_en_validacion()
    {
        /// <summary>
        /// ED-16 Comportamiento ante formatos incorrectos
        /// Si el hash o la sal almacenados presentan un formato no válido,
        /// el método deberá devolver false.
        /// <summary>
        /// Se prueba que con un formato incorrecto (hash y sal) mas corto o 
        /// mas largo que los generados no se devuelve false
        string ED_16_texto = "Cl4ve_de_prueba";
        string? ED_16_Sal = null;
        string? ED_16_Hash = null;
        string? ED_16_nuevaSal = null;
        string? ED_16_nuevoHash = null;
        Boolean? resultado = null;
//        ServicioHash ED_16_servicio = new ServicioHash();
        ResultadoHash ED_16_resultado = ServicioHash.Generar(ED_16_texto);
        ED_16_Sal = ED_16_resultado.Sal;
        ED_16_Hash = ED_16_resultado.Hash;

        // Hash corto
        ED_16_nuevoHash = ED_16_Hash[1..];
        resultado = ServicioHash.Verificar(ED_16_texto, ED_16_nuevoHash, ED_16_Sal);
        Assert.IsFalse(resultado);

        // Hash largo
        ED_16_nuevoHash = 'a' + ED_16_Hash;
        resultado = ServicioHash.Verificar(ED_16_texto, ED_16_nuevoHash, ED_16_Sal);
        Assert.IsFalse(resultado);

        // Hash mucho mas corto
        ED_16_nuevoHash = ED_16_Hash[(ED_16_Hash.Length / 2)..];
        resultado = ServicioHash.Verificar(ED_16_texto, ED_16_nuevoHash, ED_16_Sal);
        Assert.IsFalse(resultado);

        // Hash mucho mas largo
        ED_16_nuevoHash = ED_16_Hash + ED_16_Hash;
        resultado = ServicioHash.Verificar(ED_16_texto, ED_16_nuevoHash, ED_16_Sal);
        Assert.IsFalse(resultado);

        // Sal Corta
        ED_16_nuevaSal = ED_16_Sal[1..];
        resultado = ServicioHash.Verificar(ED_16_texto, ED_16_Hash, ED_16_nuevaSal);
        Assert.IsFalse(resultado);

        // Sal larga
        ED_16_nuevaSal = 'a' + ED_16_Sal;
        resultado = ServicioHash.Verificar(ED_16_texto, ED_16_Hash, ED_16_nuevaSal);
        Assert.IsFalse(resultado);

        // Sal mucho mas corta
        resultado = ServicioHash.Verificar(ED_16_texto, ED_16_Hash, ED_16_nuevaSal);
        Assert.IsFalse(resultado);

        // Sal mucho mas larga
        ED_16_nuevaSal = ED_16_Sal + ED_16_Sal;
        resultado = ServicioHash.Verificar(ED_16_texto, ED_16_Hash, ED_16_nuevaSal);
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void Robustez_de_validacion()
    {
        /// <summary>
        /// ED-17 Robustez
        /// El método no deberá propagar excepciones provocadas por datos
        /// almacenados incorrectos.
        /// <summary>
        /// Se genera una sal y un hash a partir de un texto y se prueba que
        /// la verificacionde que cambiando cada uno de los tres valores 
        /// no se lanza una excepcion
        string ED_17_texto = "Cl4ve_de_prueba";
        string ED_17_texto2 = "Cl4ve_de_prueb4";
        string? ED_17_Sal = null;
        string? ED_17_Hash = null;
        string? ED_17_nuevaSal = null;
        string? ED_17_nuevoHash = null;
        Boolean? resultado = false;
//        ServicioHash ED_17_servicio = new ServicioHash();
        ResultadoHash ED_17_resultado = ServicioHash.Generar(ED_17_texto);
        ED_17_Sal = ED_17_resultado.Sal;
        ED_17_Hash = ED_17_resultado.Hash;
        ED_17_nuevaSal = (ED_17_Sal[0] == 'a' ? 'b' : 'a') + ED_17_Sal[1..];
        ED_17_nuevoHash = (ED_17_Hash[0] == 'a' ? 'b' : 'a') + ED_17_Hash[1..];
        try
        {
            resultado = ServicioHash.Verificar(ED_17_texto2, ED_17_Hash, ED_17_Sal); // cambia la clave
            Assert.IsNotNull(resultado);
        }
        catch { Assert.Fail(); }

        try
        {
            resultado = ServicioHash.Verificar(ED_17_texto, ED_17_Hash, ED_17_nuevaSal); // cambia la salt
            Assert.IsNotNull(resultado);
        }
        catch { Assert.Fail(); }

        try
        {
            resultado = ServicioHash.Verificar(ED_17_texto, ED_17_nuevoHash, ED_17_Sal); // cambia el hash
            Assert.IsNotNull(resultado);
        }
        catch { Assert.Fail(); }
    }
}
