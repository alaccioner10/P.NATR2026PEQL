using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Expedientes;


namespace SGE.Infraestructura.Repositorios;

public class ExpedienteTxtRepository : IExpedienteRepository
{

    private readonly string _archivo= "expedientes.txt";

    public void Agregar (Expediente expediente)
    {
        string linea = $"{expediente.Id}|{expediente.Caratula.Valor}|{expediente.Estado}|{expediente.FechaCreacion}|{expediente.FechaUltimaModificacion}|{expediente.UsuarioUltimoCambio}";


        using (StreamWriter sw = new StreamWriter(_archivo, true))
        {
            sw.WriteLine(linea);
        }
    }

public IEnumerable<Expediente> ObtenerTodos()
    {
        var lista = new List<Expediente>();

        if (!File.Exists(_archivo)) return lista;

        string[] lineas = File.ReadAllLines(_archivo);

        foreach (var linea in lineas)
        {
            lista.Add(MapearDesdeLinea(linea));
        }

        return lista;
    }

    public Expediente? ObtenerPorId(Guid id)
    {
        if (!File.Exists(_archivo)) return null;

        string[] lineas = File.ReadAllLines(_archivo);
        
        foreach (var linea in lineas)
        {
            if (linea.StartsWith(id.ToString()))
            {
                return MapearDesdeLinea(linea);
            }
        }

        return null; 
    }

public void Modificar(Expediente expediente)
    {
        if (!File.Exists(_archivo)) return;

        string[] lineas = File.ReadAllLines(_archivo);
        var lineasActualizadas = new List<string>();

        foreach (var linea in lineas)
        {
            if (linea.StartsWith(expediente.Id.ToString()))
            {
                string nuevaLinea = $"{expediente.Id}|{expediente.Caratula.Valor}|{expediente.Estado}|{expediente.FechaCreacion}|{expediente.FechaUltimaModificacion}|{expediente.UsuarioUltimoCambio}";
                lineasActualizadas.Add(nuevaLinea);
            }
            else
            {
                lineasActualizadas.Add(linea);
            }
        }

        File.WriteAllLines(_archivo, lineasActualizadas);
    }

    public void Eliminar(Guid id)
    {
        if (!File.Exists(_archivo)) return;

        string[] lineas = File.ReadAllLines(_archivo);
        var lineasActualizadas = new List<string>();

        foreach (var linea in lineas)
        {
            if (!linea.StartsWith(id.ToString()))
            {
                lineasActualizadas.Add(linea);
            }
        }

        File.WriteAllLines(_archivo, lineasActualizadas);
    }

    private Expediente MapearDesdeLinea(string linea)
    {
        string[] campos = linea.Split('|');

        Guid id = Guid.Parse(campos[0]);
        Caratula caratula = new Caratula(campos[1]); 
        EstadoEnum estado = Enum.Parse<EstadoEnum>(campos[2]); 
        DateTime fechaCreacion = DateTime.Parse(campos[3]);
        DateTime fechaModificacion = DateTime.Parse(campos[4]);
        Guid usuarioId = Guid.Parse(campos[5]);

        return new Expediente(id, fechaCreacion, fechaModificacion, usuarioId, caratula, estado);
    }
}
