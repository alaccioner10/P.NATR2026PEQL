using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public interface ITramiteRepository
{
    void Agregar(Tramite tramite);
    Tramite? ObtenerPorId(Guid id);
    void Eliminar(Guid id);
    IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId);
}