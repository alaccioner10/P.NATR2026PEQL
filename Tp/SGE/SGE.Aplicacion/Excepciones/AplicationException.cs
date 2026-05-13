namespace SGE.Aplicacion.Excepciones;

public class AplicationException : Exception
{
    public AplicationException()
    {
        
    }
    public AplicationException(string? message) : base(message)
    {
        
    }
    public AplicationException(string? message, Exception? innerException) : base(message, innerException)
    {
        
    }
}