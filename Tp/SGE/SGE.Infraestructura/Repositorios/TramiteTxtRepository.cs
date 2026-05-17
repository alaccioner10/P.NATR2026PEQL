using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites;

namespace SGE.Infraestructura.Repositorios;

public class TramiteTxtRepository : ITramiteRepository
{
    private readonly string _archivo = "tramites.txt";

    public void Agregar(Tramite tramite)
    {
        string linea = $"{tramite.Id}|{tramite.ExpedienteId}|{tramite.Etiqueta}|{tramite.Contenido.Valor}|{tramite.FechaCreacion}|{tramite.FechaUltModificacion}|{tramite.UsuarioUltCambio}";

        using (StreamWriter sw = new StreamWriter(_archivo, true))
        {
            sw.WriteLine(linea);
        }
    }

    public Tramite? ObtenerPorId(Guid id)
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

    public void Modificar(Tramite tramite)
    {
        if (!File.Exists(_archivo)) return;

        string[] lineas = File.ReadAllLines(_archivo);
        var lineasActualizadas = new List<string>();

        foreach (var linea in lineas)
        {
            if (linea.StartsWith(tramite.Id.ToString()))
            {
                string nuevaLinea = $"{tramite.Id}|{tramite.ExpedienteId}|{tramite.Etiqueta}|{tramite.Contenido.Valor}|{tramite.FechaCreacion}|{tramite.FechaUltModificacion}|{tramite.UsuarioUltCambio}";
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

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        var lista = new List<Tramite>();

        if (!File.Exists(_archivo)) return lista;

        string[] lineas = File.ReadAllLines(_archivo);

        foreach (var linea in lineas)
        {
            Tramite tramite = MapearDesdeLinea(linea);

            if (tramite.ExpedienteId == expedienteId)
            {
                lista.Add(tramite);
            }
        }

        return lista;
    }

    private Tramite MapearDesdeLinea(string linea)
    {
        string[] campos = linea.Split('|');

        Guid id = Guid.Parse(campos[0]);
        Guid expedienteId = Guid.Parse(campos[1]);
        EtiquetaEnum etiqueta = Enum.Parse<EtiquetaEnum>(campos[2]); 
        Contenido contenido = new Contenido(campos[3]); 
        DateTime fechaCreacion = DateTime.Parse(campos[4]);
        DateTime fechaModificacion = DateTime.Parse(campos[5]);
        Guid usuarioId = Guid.Parse(campos[6]);

        return new Tramite(id, expedienteId, etiqueta, contenido, fechaCreacion, fechaModificacion, usuarioId);
    }
}