using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;
using SGE.Infraestructura.Excepciones;

namespace SGE.Infraestructura.Repositorios;

public class UsuarioRepository : IUsuarioRepository
{
    private SGEContext _context;
    public UsuarioRepository(SGEContext context)
    {
        _context = context;
    }
    public void Agregar(Usuario user)
    {
        _context.Usuarios.Add(user);
    }

    public void Modificar(Usuario user)
    {
        _context.Usuarios.Update(user);
    }

    public Usuario? ObtenerPorId(Guid id)
    {
        return _context.Usuarios.FirstOrDefault(e => e.Id == id);
    }
    public void Eliminar(Guid id)
    {
        Usuario? user = _context.Usuarios.FirstOrDefault(e => e.Id.Equals(id));
        if(user == null)
        {
            throw new RepositoryException("No existe el usuario");
        }
        _context.Usuarios.Remove(user);
    }

    public Usuario? ObtenerPorEmail(string email)
    {
        return _context.Usuarios.FirstOrDefault(u => u.Email == email);
    }

    public bool ExistePorEmail(string email)
    {
        return _context.Usuarios.Any(u => u.Email == email);
    }
}