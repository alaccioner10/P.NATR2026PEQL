using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;
using SGE.Infraestructura.Excepciones;

namespace SGE.Infraestructura.Repositorios;

public class TramiteRepository : ITramiteRepository
{
    private SGEContext _context;
    public TramiteRepository(SGEContext context)
    {
        _context = context;
    }
    public void Agregar(Tramite tramite)
    {
        SGESqlite.Inicializar();
        _context.Tramites.Add(tramite);
    }
    
    public Tramite? ObtenerPorId(Guid id)
    {
        SGESqlite.Inicializar();
        return _context.Tramites.FirstOrDefault(t => t.Id == id);
    }
    public IEnumerable<Tramite> ObtenerTodos()
    {
        SGESqlite.Inicializar();
        return _context.Tramites.ToList();
    }

    public void Modificar(Tramite tramite)
    {
        SGESqlite.Inicializar();
        Tramite? tra = _context.Tramites.FirstOrDefault(e => e.Id.Equals(tramite.Id));
        if(tra == null)
        {
            throw new RepositoryException("No existe el tramite");
        }
        _context.Tramites.Update(tramite);
        }
    }

    public void Eliminar(Guid id)
    {
        SGESqlite.Inicializar();
        Tramite? tr = _context.Tramites.FirstOrDefault(e => e.Id.Equals(id));
        if(tr == null)
        {
            throw new RepositoryException("No existe el tramite");
        }
        _context.Tramites.Remove(tr);
    }

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        SGESqlite.Inicializar();
        return _context.Tramites.Where(t => t.ExpedienteId == expedienteId);
    }
}