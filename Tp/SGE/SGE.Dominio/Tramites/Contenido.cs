namespace SGE.Dominio.Tramites;
using SGE.Dominio.Excepciones;

public record class Contenido
{
    public string Valor{ get; }
    public Contenido(string valor)
    {
      if (string.IsNullOrWhiteSpace(valor)){
        throw new DomainException("El nombre del contenido no puede estar vacío.");
      }
      Valor = valor.Trim();
    }

    public override string ToString()=>Valor;
}