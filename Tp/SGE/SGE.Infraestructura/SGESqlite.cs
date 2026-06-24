namespace SGE.Infraestructura;

public static class SGESqlite
{
    private static bool _initialized;
    private static readonly object _lock = new();

    public static void Inicializar(SGEContext context)
    {
        if (_initialized)
        {
            return;
        }

        lock (_lock)
        {
            if (_initialized)
            {
                return;
            }

            if (context.Database.EnsureCreated())
            {
                Console.WriteLine("Se creó base de datos");
            }

            _initialized = true;
        }
    }
}