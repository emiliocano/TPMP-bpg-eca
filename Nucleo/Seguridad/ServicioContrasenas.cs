using System;
using System.Collections.Generic;
using System.Text;

namespace TPMP_bpg_eca.Nucleo.Seguridad;

/// <summary>
/// Proporciona operaciones relacionadas con contraseñas.
/// </summary>
public static class ServicioContrasenas
{
    /// <summary>
    /// Determina si una contraseña cumple la política de seguridad.
    /// </summary>
    /// <param name="contrasena">
    /// Contraseña a validar.
    /// </param>
    /// <returns>
    /// True si la contraseña es válida; en caso contrario false.
    /// </returns>
    public static bool EsValida(string contrasena)
    {
        return !string.IsNullOrWhiteSpace(contrasena)
            && contrasena.Length >= 12
            && contrasena.Any(char.IsUpper)
            && contrasena.Any(char.IsLower)
            && contrasena.Any(char.IsDigit)
            && contrasena.Any(c => !char.IsLetterOrDigit(c));
    }
}