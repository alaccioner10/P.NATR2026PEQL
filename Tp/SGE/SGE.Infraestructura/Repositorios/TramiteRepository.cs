using System.Data.Common;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;
using SGE.Infraestructura.Excepciones;

namespace SGE.Infraestructura.Repositorios;

public class TramiteMemoriaRepository : ITramiteRepository
{
    public void Agregar(Tramite tramite)
    {
        SGESqlite.Inicializar();
        using(var db=new SGEContext())
        {
            db.Tramites.Add(tramite);
            db.SaveChanges();
        }
    }
    
    public Tramite? ObtenerPorId(Guid id)
    {
        Tramite? tramite;
        SGESqlite.Inicializar();
        using(var db=new SGEContext())
        {
            tramite=db.Tramites.FirstOrDefault(t => t.Id == id);
        }
        return tramite;
    }
    public IEnumerable<Tramite> ObtenerTodos()
    {
        SGESqlite.Inicializar();
        IEnumerable<Tramite> tramites = [];
        using(var db=new SGEContext())
        {
           tramites = db.Tramites.ToList();
        }
        return tramites;
    }

    public void Modificar(Tramite tramite)
    {
        SGESqlite.Inicializar();
        using(var db=new SGEContext())
        {
            Tramite? tra = db.Tramites.FirstOrDefault(e => e.Id.Equals(tramite.Id));
            if(tra == null)
            {
                throw new RepositoryException("No existe el expediente")
            }
            db.Tramites.Update(tramite);
            db.SaveChanges();
        }
    }

    public void Eliminar(Guid id)
    {
        SGESqlite.Inicializar();
        using(var db=new SGEContext())
        {
            Tramite? tr = db.Tramites.FirstOrDefault(e => e.Id.Equals(id));
            if(tr == null)
            {
                throw new RepositoryException("No existe el expediente")
            }
            db.Tramites.Remove(tr);
            db.SaveChanges();
        }
    }

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        IEnumerable<Tramite> tramite;
        SGESqlite.Inicializar();
        using(var db=new SGEContext())
        {
            tramite=db.Tramites.Where(t => t.ExpedienteId == expedienteId);
        }
        return tramite;
    }
}