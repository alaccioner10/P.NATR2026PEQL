using SGE.Dominio.Usuarios;

public interface ITokenProvider
{
    string GenerarToken(Usuario user);
}