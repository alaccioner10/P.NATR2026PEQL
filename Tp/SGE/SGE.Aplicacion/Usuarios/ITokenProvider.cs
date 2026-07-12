using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public interface ITokenProvider
{
    string GenerarToken(Usuario user);
}