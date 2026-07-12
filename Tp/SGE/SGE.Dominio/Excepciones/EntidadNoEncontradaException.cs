namespace SGE.Dominio.Excepciones;

public class EntidadNoEncontradaException : Exception
{
    public EntidadNoEncontradaException(string mensaje) : base(mensaje) { }
}