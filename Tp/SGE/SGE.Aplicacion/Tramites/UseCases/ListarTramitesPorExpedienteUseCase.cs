using System;
using System.Collections.Generic;
using SGE.Aplicacion.Tramites.DTOs;

namespace SGE.Aplicacion.Tramites.UseCases;

public class ListarTramitesPorExpedienteUseCase
{
    private readonly ITramiteRepository _tramiteRepo;

    public ListarTramitesPorExpedienteUseCase(ITramiteRepository tramiteRepo)
    {
        _tramiteRepo=tramiteRepo;
    }

    public List<ListarTramitesPorExpedienteResponse> Ejecutar(ListaTramitesPorExpedienteRequest req)
    {
        if (req == null)
        {
            throw new Exception("La solicitud no puede estar vacía");
        }

        var tramites= _tramiteRepo.ObtenerPorExpedienteId(req.ExpedienteID);

        var listaResultados = new List<ListarTramitesPorExpedienteResponse>();

        foreach (var t in tramites)
        {
            var dto = new ListarTramitesPorExpedienteResponse(t.Id, t.ExpedienteId, t.Etiqueta,t.Contenido.Valor,t.FechaCreacion,t.FechaUltModificacion,t.UsuarioUltCambio);

            listaResultados.Add(dto);
        }

        return listaResultados;
    }
}
