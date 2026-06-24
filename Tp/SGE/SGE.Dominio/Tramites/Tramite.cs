using SGE.Dominio.Excepciones;
using SGE.Dominio.Expedientes;

namespace SGE.Dominio.Tramites;

public class Tramite
{
    public Guid Id { get; private set; }
    public Guid ExpedienteId { get; private set; }
    public EtiquetaTramite Etiqueta { get; private set; }
    public Contenido Contenido { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltModificacion { get; private set; }
    public Guid UsuarioUltCambio { get; private set; }

    private Tramite()
    {
        Contenido = null!;
        Etiqueta = default!;
        ExpedienteId = default!;
    }

    public Tramite(Guid expediente, EtiquetaTramite etiqueta, Contenido contenido, Guid user)
        : this(Guid.NewGuid(), expediente, etiqueta, contenido, DateTime.Now, DateTime.Now, user)
    {
    }

    private Tramite(Guid id, Guid exp, EtiquetaTramite etiqueta, Contenido contenido, DateTime fechaCreacion, DateTime ultModificacion, Guid user)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("El Id no puede ser vacío.");
        }

        if (ultModificacion.CompareTo(fechaCreacion) < 0)
        {
            throw new DomainException("La fecha de modificación no puede ser previa a la de creación.");
        }

        Id = id;
        ExpedienteId = exp;
        Etiqueta = etiqueta;
        Contenido = contenido;
        FechaCreacion = fechaCreacion;
        FechaUltModificacion = ultModificacion;
        UsuarioUltCambio = user;
    }

    public static Tramite Reconstruir(Guid id, Guid exp, EtiquetaTramite etiqueta, Contenido contenido, DateTime fechaCreacion, DateTime ultModificacion, Guid user)
    {
        return new Tramite(id, exp, etiqueta, contenido, fechaCreacion, ultModificacion, user);
    }

    public void ModificarContenido(Contenido nuevoContenido, Guid usuarioId)
    {
        Contenido = nuevoContenido;
        UsuarioUltCambio = usuarioId;
        FechaUltModificacion = DateTime.Now;
    }
}
