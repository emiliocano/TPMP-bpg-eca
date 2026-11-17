using BackEnd;
using TPMP_bpg_eca.Nucleo.Modelo;

namespace www.Utilidades;

internal static class SesionAplicacion
{
    public const string SinProyecto = "Sin proyecto seleccionado";

    public static Proyecto? LeerProyectoActual(this ISession sesion, IDatos datos)
    {
        if (sesion.GetInt32(ClavesSesion.ProyectoActualId) is not int id)
        {
            return null;
        }

        Proyecto? proyecto = datos.LeerProyecto(id);
        if (proyecto is { Activo: true })
        {
            return proyecto;
        }

        sesion.Remove(ClavesSesion.ProyectoActualId);
        return null;
    }

    public static string ObtenerNombreVisible(Usuario usuario)
    {
        string nombre = usuario.NombreCompleto.Trim();
        return nombre.Length == 0 ? usuario.EMail : nombre;
    }
}
