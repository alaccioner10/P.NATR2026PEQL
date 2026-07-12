using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Usuarios.DTOs;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class ListarUsuariosUseCase
{
    private readonly IUsuarioRepository _userRepo;

    public ListarUsuariosUseCase(IUsuarioRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public List<ConsultarUsuarioResponseDTO> Ejecutar(Guid idUsuarioSolicitante)
    {
        var solicitante = _userRepo.ObtenerPorId(idUsuarioSolicitante);
        if (solicitante == null || !solicitante.EsAdmin)
        {
            throw new AutorizacionException("Solo un administrador puede listar usuarios.");
        }

        var usuarios = _userRepo.ObtenerTodos();
        return usuarios.Select(u => new ConsultarUsuarioResponseDTO(
            u.Id,
            u.Nombre,
            u.Email,
            u.EsAdmin,
            u.Permisos
        )).ToList();
    }
}