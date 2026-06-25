using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using SGE.Dominio.Excepciones;
using SGE.Dominio.Tramites;

namespace SGE.Dominio.Expedientes;

public class Expediente
{
    public Guid Id { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }
    public Caratula Caratula { get; private set; }
    public EstadoExpediente Estado { get; private set; }

    private Expediente()
    {
        Caratula = null!;
    }

    public Expediente(Caratula caratula, Guid user)
        : this(Guid.NewGuid(), DateTime.Now, DateTime.Now, user, caratula, EstadoExpediente.RecienIniciado)
    {
    }

    private Expediente(Guid id, DateTime fechaCreacion, DateTime fechaUltimaModificacion, Guid usuarioUltimoCambio, Caratula caratula, EstadoExpediente estado)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("El Id no puede ser vacío.");
        }

        if (fechaUltimaModificacion.CompareTo(fechaCreacion) < 0)
        {
            throw new DomainException("La fecha de modificación no puede ser previa a la de creación.");
        }

        Id = id;
        FechaCreacion = fechaCreacion;
        FechaUltimaModificacion = fechaUltimaModificacion;
        UsuarioUltimoCambio = usuarioUltimoCambio;
        Caratula = caratula;
        Estado = estado;
    }

    public static Expediente Reconstruir(Guid id, DateTime fechaCreacion, DateTime fechaUltimaModificacion, Guid usuarioUltimoCambio, Caratula caratula, EstadoExpediente estado)
    {
        return new Expediente(id, fechaCreacion, fechaUltimaModificacion, usuarioUltimoCambio, caratula, estado);
    }

    private void RegistrarModificacion(Guid idUser)
    {
        UsuarioUltimoCambio = idUser;
        FechaUltimaModificacion = DateTime.Now;
    }

    public void ModificarCaratula(Caratula nuevaC, Guid idUser)
    {
        Caratula = nuevaC;
        RegistrarModificacion(idUser);
    }

    public bool ActualizarEstado(EtiquetaTramite? ultEtiqueta, Guid idUser)
    {
        EstadoExpediente estadoAntes = Estado;
        switch (ultEtiqueta)
        {
            case null:
                Estado = EstadoExpediente.RecienIniciado;
                break;
            case EtiquetaTramite.Resolucion:
                Estado = EstadoExpediente.ConResolucion;
                break;
            case EtiquetaTramite.PaseAEstudio:
                Estado = EstadoExpediente.ParaResolver;
                break;
            case EtiquetaTramite.PaseAlArchivo:
                Estado = EstadoExpediente.Finalizado;
                break;
        }

        RegistrarModificacion(idUser);
        return estadoAntes != Estado;
    }

    public bool ActualizarEstado(EstadoExpediente nuevoEstado, Guid idUser)
    {
        EstadoExpediente estadoAntes = Estado;
        Estado = nuevoEstado;
        RegistrarModificacion(idUser);
        return estadoAntes != Estado;
    }
}
