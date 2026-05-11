using SGE.Dominio.Excepciones;

namespace SGE.Dominio.Expedientes;

public record class Caratula
{
    public string Valor{ get; }
    public Caratula(string valor)
    {
      if (string.IsNullOrWhiteSpace(valor)){
        throw new DomainException("El nombre de la caratula no puede estar vacío.");
      }
      Valor = valor.Trim();
    }

    public override string ToString()=>Valor;
}