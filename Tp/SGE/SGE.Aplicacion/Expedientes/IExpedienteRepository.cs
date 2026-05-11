using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public interface IExpedienteRepository
{
    void Agregar(Expediente expediente);
    Expediente? ObtenerPorId(Guid id);
    void Modificar(Expediente expediente);
    void Eliminar(Guid id);
    IEnumerable<Expediente> ObtenerTodos();
}