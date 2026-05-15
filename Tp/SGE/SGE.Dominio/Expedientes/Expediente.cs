using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using SGE.Dominio.Excepciones;
using SGE.Dominio.Tramites;

namespace SGE.Dominio.Expedientes;

public class Expediente
{
    public Guid Id {get; private set;}
    public DateTime FechaCreacion {get; private set;}
    public DateTime FechaUltimaModificacion {get; private set;}
    public Guid UsuarioUltimoCambio {get; private set;}
    public Caratula Caratula{get; private set;}
    public EstadoEnum Estado {get; private set;}

    /*
        Constructor de expediente NUEVO, solo recibe cosas que no puede generar por su cuenta.
    */
    public Expediente(Caratula caratula, Guid user) : this(Guid.NewGuid(), DateTime.Now, DateTime.Now, user, caratula, EstadoEnum.RecienIniciado)
    {
    }

    /*
        Constructor de expendiente cuando es RECUPERADO u OBTENIDO de otro lado con todos sus datos,
        aca no genera nada por su cuenta.
    */
    private Expediente(Guid id, DateTime fechaCreacion, DateTime fechaUlt, Guid usuarioUlt, Caratula caratula, EstadoEnum estado)
    {
        if(id == Guid.Empty)
        {
            throw new DomainException("El Id no puede ser vacío.");
        }

        if(fechaUlt.CompareTo(fechaCreacion) < 0)
        {
            throw new DomainException("La fecha de modificación no puede ser previa a la de creación.");
        }

        Id=id;
        FechaCreacion = fechaCreacion;
        FechaUltimaModificacion = fechaUlt;
        UsuarioUltimoCambio = usuarioUlt;
        Caratula = caratula;
        Estado = estado;
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

    public bool ActualizarEstado(EtiquetaEnum? ultEtiqueta, Guid idUser)
    {
        EstadoEnum estadoAntes = Estado;
        switch (ultEtiqueta)
        {
            case null:
                Estado = EstadoEnum.RecienIniciado;
                break;
            case EtiquetaEnum.Resolucion:
                Estado = EstadoEnum.ConResolucion;
                break;
            case EtiquetaEnum.PaseAEstudio:
                Estado= EstadoEnum.ParaResolver;
                break;
            case EtiquetaEnum.PaseAlArchivo:
                Estado = EstadoEnum.Finalizado;
                break;
        }
        RegistrarModificacion(idUser);
        return estadoAntes == Estado;
    }

    public void CambiarEstado(EstadoEnum nuevoEst, Guid idUser)
    {
        Estado = nuevoEst;
        RegistrarModificacion(idUser);
    }


    //Preguntar si esta bien hacer esto, estamos llamndo al constructor desde afuera para reconstruir el archivo txt. 
    public static Expediente Reconstruir(Guid id, DateTime fechaCreacion, DateTime fechaUlt, Guid usuarioUlt, Caratula caratula, EstadoEnum estado)
    {
        // Acá adentro sí podemos usar el constructor privado
        return new Expediente(id, fechaCreacion, fechaUlt, usuarioUlt, caratula, estado);
    }
}