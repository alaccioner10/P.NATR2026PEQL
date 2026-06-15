using System.Security.Cryptography;
using System.Text;

namespace SGE.Aplicacion.Usuarios;

public static class ContrasenaUtil
{
    public static string Convertir(string contrasena)
    {
        byte[] hash=SHA256.HashData(Encoding.UTF8.GetBytes(contrasena));

        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public static bool Comparar(string contrasena,string hash)
    {
        return Convertir(contrasena).Equals(hash);
    }
}