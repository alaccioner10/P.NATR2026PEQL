namespace SGE.Dominio.Usuarios;

using SGE.Dominio.Excepciones;
using SGE.Dominio.Usuarios;
public class Usuario
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public string Email { get; private set; }
    public string ContrasenaHash { get; private set; }
    public bool EsAdmin { get; private set; }
    public IEnumerable<Permiso> Permisos { get; private set; } = [];

    protected Usuario()
    {
        Nombre = string.Empty;
        Email = string.Empty;
        ContrasenaHash = string.Empty;
    }

    public Usuario(string nombre, string email, string contrasenaHash) : this(Guid.NewGuid(), nombre, email, contrasenaHash, false, [])
    {
    }

    private Usuario(Guid id, string nombre, string email, string contrasenaHash, bool esAdmin, IEnumerable<Permiso> permisos)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("El Id no puede ser vacío.");
        }
        Id = id;
        Nombre = nombre;
        Email = email;
        ContrasenaHash = contrasenaHash;
        EsAdmin = esAdmin;
        Permisos = permisos.Distinct().ToArray();
    }

    public static Usuario Reconstruir(Guid id, string nombre, string email, string contrasenaHash, bool esAdmin, IEnumerable<Permiso> permisos)
    {
        return new Usuario(id, nombre, email, contrasenaHash, esAdmin, permisos);
    }

    public void CambiarNombre(string nombre)
    {
        Nombre = nombre;
    }

    public void CambiarEmail(string email)
    {
        Email = email;
    }

    public void CambiarContrasenaHash(string contrasenaHash)
    {
        ContrasenaHash = contrasenaHash;
    }

    public void AsignarPermiso(Permiso permiso)
    {
        if (Permisos.Contains(permiso))
        {
            return;
        }

        Permisos = Permisos.Append(permiso).ToArray();
    }

    public void RemoverPermiso(Permiso permiso)
    {
        Permisos = Permisos.Where(p => p != permiso).ToArray();
    }
}