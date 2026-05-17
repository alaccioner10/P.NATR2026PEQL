using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;

namespace SGE.Infraestructura.Repositorios;

public class TramiteMemoriaRepository : ITramiteRepository
{
    private static readonly List<Tramite> _tramites = new List<Tramite>();

    public void Agregar(Tramite tramite) => _tramites.Add(tramite);
    
    public Tramite? ObtenerPorId(Guid id) => _tramites.FirstOrDefault(t => t.Id == id);
    
    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId) 
        => _tramites.Where(t => t.ExpedienteId == expedienteId);

    public void Modificar(Tramite tramite)
    {
        var index = _tramites.FindIndex(t => t.Id == tramite.Id);
        if (index != -1) _tramites[index] = tramite;
    }

    public void Eliminar(Guid id) => _tramites.RemoveAll(t => t.Id == id);
}