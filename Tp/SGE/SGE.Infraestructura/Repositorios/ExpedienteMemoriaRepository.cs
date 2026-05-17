using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;

namespace SGE.Infraestructura.Repositorios;

public class ExpedienteMemoriaRepository : IExpedienteRepository
{
    private static readonly List<Expediente> _expedientes = new List<Expediente>();

    public void Agregar(Expediente expediente) => _expedientes.Add(expediente);
    
    public Expediente? ObtenerPorId(Guid id) => _expedientes.FirstOrDefault(e => e.Id == id);
    
    public IEnumerable<Expediente> ObtenerTodos() => _expedientes;

    public void Modificar(Expediente expediente)
    {
        var index = _expedientes.FindIndex(e => e.Id == expediente.Id);
        if (index != -1) _expedientes[index] = expediente;
    }

    public void Eliminar(Guid id) => _expedientes.RemoveAll(e => e.Id == id);
}