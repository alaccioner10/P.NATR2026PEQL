using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public interface IUsuarioRepository
{
    void Agregar(Usuario user);
    Usuario? ObtenerPorId(Guid id);
    void Modificar(Usuario user);
    void Eliminar(Guid id);
    Usuario? ObtenerPorEmail(string email);
    bool ExistePorEmail(string email);
}