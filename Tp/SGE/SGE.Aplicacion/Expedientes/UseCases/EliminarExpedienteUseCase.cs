using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Expedientes.DTOs; 
using SGE.Aplicacion.Expedientes;     

namespace SGE.Aplicacion.Expedientes.UseCases;

public class EliminarExpedienteUseCase
{
    private readonly IExpedienteRepository _repoExpediente;
    private readonly ITramiteRepository _repoTramite;
    private readonly IAutorizacionService _autorizacion;

    public EliminarExpedienteUseCase(
        IExpedienteRepository repoExpediente,
        ITramiteRepository repoTramite,
        IAutorizacionService autorizacion
    )
    {
        _repoExpediente = repoExpediente;
        _repoTramite = repoTramite;
        _autorizacion = autorizacion;
    }

    public void Ejecutar(EliminarExpedienteRequest request)
    {

        if (!_autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteBaja))
        {
            throw new AutorizacionException("El usuario no tiene permisos para eliminar expedientes.");
        }


        var tramitesDelExpediente = _repoTramite.ObtenerPorExpedienteId(request.ExpedienteId);

        foreach (var tramite in tramitesDelExpediente)
        {
            _repoTramite.Eliminar(tramite.Id);
        }

        _repoExpediente.Eliminar(request.ExpedienteId);
    }
}