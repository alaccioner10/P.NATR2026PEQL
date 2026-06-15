namespace SGE.Dominio.Usuarios;
using SGE.Dominio.Excepciones;
public class Usuario
{
    public Guid Id{get; private set;}
    public string Nombre{get; private set;}
    public string Email{get; private set;}
    public string ContrasenaHash{get;private set;}
    public bool EsAdmin{get;private set;}
    public IEnumerable<Permiso> Permisos{get;private set;}

    public Usuario(string nombre, string email, string contrasenaHash) : this(Guid.NewGuid(), nombre, email, contrasenaHash,false,[])
    {
    }

    public Usuario(Guid id,string nombre, string email, string contrasenaHash, bool esAdmin, IEnumerable<Permiso> permisos)
    {
        if(id == Guid.Empty)
        {
            throw new DomainException("El Id no puede ser vacío.");
        }
        Id=id;
        Nombre=nombre;
        Email=email;
        ContrasenaHash=contrasenaHash;
        EsAdmin=esAdmin;
        Permisos=permisos;
    }
}