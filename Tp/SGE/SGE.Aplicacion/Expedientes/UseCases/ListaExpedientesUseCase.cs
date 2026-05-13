using System.Reflection.Metadata.Ecma335;
using SGE.Aplicacion.Expedientes.DTOs;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class ListaExpedientesUseCase
{
    private readonly IExpedienteRepository _iExpRepo;

    public ListaExpedientesUseCase(IExpedienteRepository iExpRepo)
    {
        _iExpRepo = iExpRepo;
    }   

    public List<ConsultarExpedienteResponse> Ejecutar(ListarExpedientesRequest req)
    {
        var expedientes = _iExpRepo.ObtenerTodos();

        var listaResultados = new List<ConsultarExpedienteResponse>();

        foreach(var exp in expedientes)
        {
            var dto=new ConsultarExpedienteResponse(exp.Id, exp.Caratula.Valor, exp.Estado.ToString(), exp.FechaCreacion, exp.FechaUltimaModificacion);

             listaResultados.Add(dto);
        }
        return listaResultados;
       
    }
}
