using System.Security.Cryptography;
using System.Text;
using SGE.Aplicacion.Servicios;

namespace SGE.Infraestructura.Servicios;

public class HashService : IHashService
{
    public string Hash(string plainText)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(plainText));
        return Convert.ToHexString(bytes).ToLower();
    }

    public bool Verify(string plainText, string hashedText)
    {
        return Hash(plainText) == hashedText;
    }
}