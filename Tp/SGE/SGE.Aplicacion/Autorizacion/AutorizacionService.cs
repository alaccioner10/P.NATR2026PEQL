using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Autorizacion;

public class AutorizacionService : IAutorizacionService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AutorizacionService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        var usuario = _usuarioRepository.ObtenerPorId(idUsuario);
        if (usuario == null)
        {
            return false;
        }

        return usuario.EsAdmin || usuario.Permisos.Contains(permiso);
    }
}
