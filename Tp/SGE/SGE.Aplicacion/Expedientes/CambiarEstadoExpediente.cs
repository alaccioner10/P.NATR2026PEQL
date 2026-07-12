using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class CambiarEstadoExpediente
{
    private readonly IUnidadDeTrabajo _uow;
    private readonly IExpedienteRepository _iExpRepo;
    private readonly IAutorizacionService _autorizacion;

    public CambiarEstadoExpediente(IUnidadDeTrabajo uow, IExpedienteRepository iExpRepo, IAutorizacionService autorizacion)
    {
        _uow = uow;
        _iExpRepo = iExpRepo;
        _autorizacion = autorizacion;
    }

    public CambiarEstadoExpResponseDTO Ejecutar(CambiarEstadoExpDTO req, Guid idUsuario)
    {
        if (req == null)
        {
            throw new AplicationException("La solicitud no puede estar vacia");
        }

        if (!_autorizacion.PoseeElPermiso(idUsuario, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permisos para modificar el estado");
        }

        var exp = _iExpRepo.ObtenerPorId(req.IdExp);
        if (exp == null)
        {
            throw new AplicationException("El expediente solicitado no existe");
        }

        exp.ActualizarEstado(req.Estado, idUsuario);

        _iExpRepo.Modificar(exp);
        _uow.Guardar();

        return new CambiarEstadoExpResponseDTO(exp.Id, exp.Estado, exp.UsuarioUltimoCambio, exp.FechaUltimaModificacion);
    }
}