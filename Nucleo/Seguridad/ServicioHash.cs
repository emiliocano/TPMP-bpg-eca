using System.Security.Cryptography;

namespace TPMP_bpg_eca.Nucleo.Seguridad;

public static class ServicioHash
{// Parámetros utilizados para generar el hash de las contraseñas:
 // sal de 16 bytes, hash de 32 bytes y 100.000 iteraciones
    private const int TamanoSal = 16;
    private const int TamanoHash = 32;
    private const int NumeroIteraciones = 100_000;

    
    internal static byte[] CrearSal()
    {
        return RandomNumberGenerator.GetBytes(TamanoSal);
    }

    public static ResultadoHash Generar(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            // ED-04 exige esta excepción cuando el texto no es válido
            throw new InvalidOperationException("El texto no puede estar vacío");
        }

        byte[] sal = CrearSal();
        byte[] hash = Calcular(texto, sal);

        return new ResultadoHash
        {
            Hash = Convert.ToBase64String(hash),
            Sal = Convert.ToBase64String(sal)
        };
    }

    public static bool Verificar(
        string texto,
        string hashAlmacenado,
        string salAlmacenada)
    {
        if (string.IsNullOrWhiteSpace(texto) ||
            string.IsNullOrWhiteSpace(hashAlmacenado) ||
            string.IsNullOrWhiteSpace(salAlmacenada))
        {
            return false;
        }

        try
        {
            byte[] sal =
                Convert.FromBase64String(salAlmacenada);

            byte[] hashEsperado =
                Convert.FromBase64String(hashAlmacenado);

            byte[] hashCalculado = Calcular(texto, sal);
            // Compara los hashes en tiempo constante para reducir
            // la información que podría revelar el tiempo de respuesta
            return CryptographicOperations.FixedTimeEquals(
                hashCalculado,
                hashEsperado);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static bool Verificar(
        string texto,
        ResultadoHash? hashAlmacenado)
    {
        return hashAlmacenado is not null &&
            Verificar(texto, hashAlmacenado.Hash, hashAlmacenado.Sal);
    }
    // Deriva el hash mediante PBKDF2 con SHA-256 y la sal indicada
    internal static byte[] Calcular(
        string texto,
        byte[] sal)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            texto,
            sal,
            NumeroIteraciones,
            HashAlgorithmName.SHA256,
            TamanoHash);
    }
}
