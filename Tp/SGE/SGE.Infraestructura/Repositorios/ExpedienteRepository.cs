using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;
using SGE.Infraestructura.Excepciones;

namespace SGE.Infraestructura.Repositorios;

public class ExpedienteRepository : IExpedienteRepository
{
    private SGEContext _context;
    public ExpedienteRepository(SGEContext context)
    {
        _context = context;
    }
    public void Agregar(Expediente expediente)
    {
        _context.Expedientes.Add(expediente);
    }
    
    public Expediente? ObtenerPorId(Guid id)
    {
        return _context.Expedientes.FirstOrDefault(e => e.Id == id);
    }
    public IEnumerable<Expediente> ObtenerTodos()
    {
        return _context.Expedientes.ToList();
    }

    public void Modificar(Expediente expediente)
    {
        _context.Expedientes.Update(expediente);
    }

    public void Eliminar(Guid id)
    {
        Expediente? exp = _context.Expedientes.FirstOrDefault(e => e.Id.Equals(id));
        if(exp == null)
        {
            throw new RepositoryException("No existe el expediente");
        }
        _context.Expedientes.Remove(exp);
    }
}