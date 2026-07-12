namespace SGE.Aplicacion.Servicios;

public interface IHashService
{
    string Hash(string plainText);
    bool Verify(string plainText, string hashedText);
}