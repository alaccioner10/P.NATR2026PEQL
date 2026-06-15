using System.Data.Common;
using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;
using SGE.Infraestructura.Excepciones;

namespace SGE.Infraestructura.Repositorios;

public class ExpedienteMemoriaRepository : IExpedienteRepository
{

    public void Agregar(Expediente expediente)
    {
        SGESqlite.Inicializar();
        using(var db=new SGEContext())
        {
            db.Expedientes.Add(expediente);
            db.SaveChanges();
        }
    }
    
    public Expediente? ObtenerPorId(Guid id)
    {
        Expediente? exp;
        SGESqlite.Inicializar();
        using(var db=new SGEContext())
        {
            exp=db.Expedientes.FirstOrDefault(e => e.Id == id);
        }
        return exp;
    }
    public IEnumerable<Expediente> ObtenerTodos()
    {
        SGESqlite.Inicializar();
        IEnumerable<Expediente> exps = [];
        using(var db=new SGEContext())
        {
           exps = db.Expedientes.ToList();
        }
        return exps;
    }

    public void Modificar(Expediente expediente)
    {
        SGESqlite.Inicializar();
        using(var db=new SGEContext())
        {
            Expediente? exp = db.Expedientes.FirstOrDefault(e => e.Id.Equals(expediente.Id));
            if(exp == null)
            {
                throw new RepositoryException("No existe el expediente")
            }
            db.Expedientes.Update(expediente);
            db.SaveChanges();
        }
    }

    public void Eliminar(Guid id)
    {
        SGESqlite.Inicializar();
        using(var db=new SGEContext())
        {
            Expediente? exp = db.Expedientes.FirstOrDefault(e => e.Id.Equals(id));
            if(exp == null)
            {
                throw new RepositoryException("No existe el expediente")
            }
            db.Expedientes.Remove(exp);
            db.SaveChanges();
        }
    }
}