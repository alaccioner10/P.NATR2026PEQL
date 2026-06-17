using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;
using SGE.Infraestructura.Excepciones;

namespace SGE.Infraestructura.Repositorios;

public class UsuarioRepository : IUsuarioRepository
{
    private SGEContext _context;
    public UsuarioRepository(SGEContext context)
    {
        _context=context;
    }
    public void Agregar(Usuario user)
    {
        SGESqlite.Inicializar();
        _context.Usuarios.Add(user);
    }

    public Usuario? ObtenerPorId(Guid id)
    {
        SGESqlite.Inicializar();
        return _context.Usuarios.FirstOrDefault(e => e.Id == id);
    }
    public void Eliminar(Guid id)
    {
        SGESqlite.Inicializar();
        Usuario? user = _context.Usuarios.FirstOrDefault(e => e.Id.Equals(id));
        if(user == null)
        {
            throw new RepositoryException("No existe el usuario");
        }
        _context.Usuarios.Remove(user);
    }

    public Usuario? ObtenerPorEmail(string email)
    {
        SGESqlite.Inicializar();
        return _context.Usuarios.FirstOrDefault(u => u.Email == email);
    }

    public bool ExistePorEmail(string email)
    {
        SGESqlite.Inicializar();
        return _context.Usuarios.Any(u => u.Email == email);
    }
}