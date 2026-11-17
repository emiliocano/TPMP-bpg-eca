using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PaginaLogin = www.Pages.Login.IndexModel;

[assembly: DoNotParallelize]

namespace WWWTest;

[TestClass]
public class AceptacionWebTest
{
    private const string CorreoAdmin = "uadmin@ubu.es";
    private const string CorreoTester = "tester1@ubu.es";
    private const string ClaveInicial = "DemoLocal26!";
    private const string ClaveIncorrecta = "Incorrecta27!";
    private const string ClaveNueva = "DemoLocal27!";
    private const string ErrorAcceso = "Usuario o contraseña incorrectos.";

    private static readonly By AvisoClave = By.CssSelector("span[data-valmsg-for='Contraseña']");
    private static readonly By MenuCuenta = By.CssSelector(".cabecera-usuario button[data-bs-toggle='dropdown']");
    private static readonly By BotonValidar = By.Id("btnAceptar");
    private static readonly By GuardarPerfil = By.CssSelector("form[action$='handler=GuardarPerfil'] button[type='submit']");
    private static readonly By CambiarClave = By.CssSelector("form[action$='handler=CambiarPassword'] button[type='submit']");

    private WebApplicationFactory<PaginaLogin> _aplicacion = null!;
    private ChromeDriver _chrome = null!;
    private WebDriverWait _espera = null!;
    private Uri _direccion = null!;

    [TestInitialize]
    public void Preparar()
    {
        // Cada prueba usa los datos iniciales y una sesión de navegador propias.
        _aplicacion = new WebApplicationFactory<PaginaLogin>();
        _aplicacion.UseKestrel(0);
        using var cliente = _aplicacion.CreateClient();
        _direccion = cliente.BaseAddress!;

        var opciones = new ChromeOptions();
        opciones.AddArguments("--headless", "--window-size=1440,1000");
        _chrome = new ChromeDriver(opciones);
        _espera = new WebDriverWait(_chrome, TimeSpan.FromSeconds(10));
        _espera.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
    }

    [TestCleanup]
    public void Cerrar()
    {
        try
        {
            _chrome?.Dispose();
        }
        finally
        {
            _aplicacion?.Dispose();
        }
    }

    [TestMethod] // KR01
    public void LoginCorrecto_MuestraLaZonaDeTrabajo()
    {
        Identificar(CorreoAdmin);
        Validar(ClaveInicial);

        Assert.AreEqual("Zona de trabajo", Visible(By.TagName("h1")).Text);
        Assert.AreEqual(CorreoAdmin, Visible(MenuCuenta).Text);
    }

    [TestMethod] // KR02
    public void ClaveIncorrecta_MuestraErrorYPermaneceEnValidacion()
    {
        Identificar(CorreoTester);
        Validar(ClaveIncorrecta);

        Assert.AreEqual(ErrorAcceso, Visible(AvisoClave).Text);
        Assert.AreEqual("Valida tu contraseña", Visible(By.TagName("h1")).Text);
    }

    [TestMethod] // KR03
    public void CorreoInexistente_MuestraElErrorDeAcceso()
    {
        Identificar("inexistente@example.test");
        Validar(ClaveInicial);

        Assert.AreEqual(ErrorAcceso, Visible(AvisoClave).Text);
    }

    [TestMethod] // KR04
    public void SegundoPasoSinIdentificacion_VuelveAlLogin()
    {
        Abrir("/Login/Index2");

        ComprobarPantallaDeLogin();
    }

    [TestMethod] // KR05
    public void PerfilSinSesion_RedirigeAlLogin()
    {
        Abrir("/Usuarios/Perfil");

        ComprobarPantallaDeLogin();
    }

    [TestMethod] // KR06
    public void CerrarSesion_ImpideVolverAPrincipal()
    {
        EntrarComoTester();
        Visible(MenuCuenta).Click();
        Visible(By.CssSelector("form[action$='handler=CerrarSesion'] button[type='submit']")).Click();

        ComprobarPantallaDeLogin();
        Abrir("/Principal");
        ComprobarPantallaDeLogin();
    }

    [TestMethod] // KR07
    public void AbrirPerfilDesdeElMenu_MuestraLaCuentaActual()
    {
        EntrarComoTester();
        AbrirPerfilDesdeElMenu();

        Assert.AreEqual("Mi perfil", Visible(By.TagName("h1")).Text);
        Assert.AreEqual(CorreoTester, Visible(By.Id("EMail")).GetDomProperty("value"));
    }

    [TestMethod] // KR08
    public void EditarPerfil_ConservaLosCambiosAlVolverAAbrirlo()
    {
        EntrarComoTester();
        AbrirPerfilDesdeElMenu();
        Escribir("Nombre", "Probador");
        Escribir("Apellidos", "de la Cruz Pérez");
        Visible(GuardarPerfil).Click();
        _ = Visible(By.CssSelector(".alert-success"));

        Abrir("/Usuarios/Perfil");
        Assert.AreEqual("Probador", Visible(By.Id("Nombre")).GetDomProperty("value"));
        Assert.AreEqual("de la Cruz Pérez", Visible(By.Id("Apellidos")).GetDomProperty("value"));
    }

    [TestMethod] // KR09
    public void GuardarCorreoDeOtraCuenta_MuestraErrorYConservaElAnterior()
    {
        EntrarComoTester();
        AbrirPerfilDesdeElMenu();
        Escribir("EMail", CorreoAdmin);
        Escribir("Nombre", "Probador");
        Escribir("Apellidos", "Selenium");
        Visible(GuardarPerfil).Click();

        Assert.AreEqual("Ya existe otro usuario con ese correo electrónico.",
            Visible(By.CssSelector("span[data-valmsg-for='EMail']")).Text);
        Abrir("/Usuarios/Perfil");
        Assert.AreEqual(CorreoTester, Visible(By.Id("EMail")).GetDomProperty("value"));
    }

    [TestMethod] // KR10
    public void ConfirmacionDistinta_RechazaElCambioDeClave()
    {
        EntrarComoTester();
        AbrirPerfilDesdeElMenu();
        EnviarCambioDeClave("DemoDistinta27!");

        Assert.AreEqual("Las nuevas contraseñas no coinciden.",
            Visible(By.CssSelector("span[data-valmsg-for='ConfirmacionPassword']")).Text);
    }

    [TestMethod] // KR11
    public void CambiarClave_RechazaLaAnteriorYAceptaLaNueva()
    {
        EntrarComoTester();
        AbrirPerfilDesdeElMenu();
        EnviarCambioDeClave(ClaveNueva);
        Assert.AreEqual("La contraseña se ha cambiado correctamente.",
            Visible(By.CssSelector(".alert-success")).Text);

        Identificar(CorreoTester);
        Validar(ClaveInicial);
        Assert.AreEqual(ErrorAcceso, Visible(AvisoClave).Text);

        Validar(ClaveNueva);
        Assert.AreEqual("Zona de trabajo", Visible(By.TagName("h1")).Text);
    }

    [TestMethod] // KR12
    public void TresFallos_BloqueanLaCuentaYDeshabilitanValidarAlVolver()
    {
        Identificar(CorreoTester);
        Validar(ClaveIncorrecta);
        Assert.AreEqual(ErrorAcceso, Visible(AvisoClave).Text);
        Validar(ClaveIncorrecta);
        Assert.AreEqual(ErrorAcceso, Visible(AvisoClave).Text);
        Validar(ClaveIncorrecta);

        Assert.AreEqual("Has fallado tres veces. Cuenta bloqueada durante 5 minutos.", Visible(AvisoClave).Text);
        Assert.IsFalse(Visible(BotonValidar).Enabled);
        Assert.IsTrue(Visible(By.Id("cuentaAtras")).Displayed);

        Identificar(CorreoTester);
        Assert.IsFalse(Visible(BotonValidar).Enabled);
        Assert.IsTrue(Visible(By.Id("cuentaAtras")).Displayed);
    }

    private IWebElement Visible(By selector) => _espera.Until(chrome =>
        chrome.FindElements(selector).FirstOrDefault(elemento => elemento.Displayed))!;

    private void Abrir(string ruta) => _chrome.Navigate().GoToUrl(new Uri(_direccion, ruta));

    private void Escribir(string id, string valor)
    {
        var campo = Visible(By.Id(id));
        campo.Clear();
        campo.SendKeys(valor);
    }

    private void Identificar(string correo)
    {
        Abrir("/Login");
        Escribir("txtUsuario", correo);
        Visible(By.Id("btnSiguiente")).Click();
    }

    private void Validar(string clave)
    {
        Escribir("txtContraseña", clave);
        Visible(BotonValidar).Click();
    }

    private void EntrarComoTester()
    {
        Identificar(CorreoTester);
        Validar(ClaveInicial);
    }

    private void ComprobarPantallaDeLogin()
    {
        Assert.AreEqual("Identifícate", Visible(By.TagName("h1")).Text);
        Assert.IsTrue(Visible(By.Id("txtUsuario")).Displayed);
    }

    private void AbrirPerfilDesdeElMenu()
    {
        Visible(MenuCuenta).Click();
        Visible(By.LinkText("Ver perfil")).Click();
    }

    private void EnviarCambioDeClave(string confirmacion)
    {
        Escribir("PasswordActual", ClaveInicial);
        Escribir("PasswordNueva", ClaveNueva);
        Escribir("ConfirmacionPassword", confirmacion);
        Visible(CambiarClave).Click();
    }
}
