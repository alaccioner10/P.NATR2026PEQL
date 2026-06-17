using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;
using SGE.Infraestructura.Excepciones;

namespace SGE.Infraestructura.Repositorios;

public class ExpedienteRepository : IExpedienteRepository
{
    private SGEContext _context;
    public ExpedienteRepository(SGEContext context)
    {
        _context=context;
    }
    public void Agregar(Expediente expediente)
    {
        SGESqlite.Inicializar();
        _context.Expedientes.Add(expediente);
    }
    
    public Expediente? ObtenerPorId(Guid id)
    {
        SGESqlite.Inicializar();
        return _context.Expedientes.FirstOrDefault(e => e.Id == id);
    }
    public IEnumerable<Expediente> ObtenerTodos()
    {
        SGESqlite.Inicializar();
        return _context.Expedientes.ToList();
    }

    public void Eliminar(Guid id)
    {
        SGESqlite.Inicializar();
        Expediente? exp = _context.Expedientes.FirstOrDefault(e => e.Id.Equals(id));
        if(exp == null)
        {
            throw new RepositoryException("No existe el expediente");
        }
        _context.Expedientes.Remove(exp);
    }
}