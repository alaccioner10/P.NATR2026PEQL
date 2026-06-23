using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public interface IExpedienteRepository
{
    void Agregar(Expediente expediente);
    void Modificar(Expediente expediente);
    Expediente? ObtenerPorId(Guid id);
    void Eliminar(Guid id);
    IEnumerable<Expediente> ObtenerTodos();
}