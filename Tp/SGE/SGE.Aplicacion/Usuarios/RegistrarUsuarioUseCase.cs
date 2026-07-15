using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Servicios;
using SGE.Aplicacion.Usuarios.DTOs;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class RegistrarUsuarioUseCase
{
    private readonly IUnidadDeTrabajo _uow;
    private readonly IUsuarioRepository _userRepo;
    private readonly IHashService _hashService;


    public RegistrarUsuarioUseCase(IUnidadDeTrabajo uow, IUsuarioRepository userRepo, IHashService hashService)
    {
        _uow = uow;
        _userRepo = userRepo;
        _hashService = hashService;
    }


    public RegistrarUsuarioResponseDTO Ejecutar(RegistrarUsuarioDTO req, Guid idUsuarioSolicitante)
    {
        var solicitante = _userRepo.ObtenerPorId(idUsuarioSolicitante);
        if (solicitante == null || !solicitante.EsAdmin)
        {
            throw new AutorizacionException("Solo un administrador puede registrar usuarios.");
        }

        if (_userRepo.ExistePorEmail(req.Email))
        {
            throw new AplicationException("El email ya está registrado");
        }
        Usuario user = new Usuario(req.Nombre, req.Email, _hashService.Hash(req.Contrasena));

        _userRepo.Agregar(user);
        _uow.Guardar();

        return new RegistrarUsuarioResponseDTO(user.Id, user.Nombre, user.Email, user.EsAdmin, user.Permisos);
    }
}