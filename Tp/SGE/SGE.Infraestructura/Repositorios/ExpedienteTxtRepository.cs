using System;
using System.Collections.Generic;
using System.IO;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Expedientes.UseCases;

namespace SGE.Infraestructura.Repositorios;

// Le decimos a C# que esta clase implementa el contrato IExpedienteRepository
public class ExpedienteTxtRepository : IExpedienteRepository
{

    // Definimos el nombre/ruta del archivo donde se va a guardar todo
    private readonly string _archivo= "expedientes.txt";

    public void Agregar (Expediente expediente)
    {
        // 1. Armamos la cadena de texto separando las propiedades por un "|"
        string linea = $"{expediente.Id}|{expediente.Caratula.Valor}|{expediente.Estado}|{expediente.FechaCreacion}|{expediente.FechaUltimaModificacion}|{expediente.UsuarioUltimoCambio}";

        // 2. Escribimos en el archivo. 
        // El 'true' significa "Append": agrega la línea al final sin borrar lo que ya estaba.
        using (StreamWriter sw = new StreamWriter(_archivo, true))
        {
            sw.WriteLine(linea);
        }
    }

public IEnumerable<Expediente> ObtenerTodos()
    {
        var lista = new List<Expediente>();

        if (!File.Exists(_archivo)) return lista;

        string[] lineas = File.ReadAllLines(_archivo);

        foreach (var linea in lineas)
        {
            lista.Add(MapearDesdeLinea(linea));
        }

        return lista;
    }

    public Expediente? ObtenerPorId(Guid id)
    {
        if (!File.Exists(_archivo)) return null;

        string[] lineas = File.ReadAllLines(_archivo);
        
        foreach (var linea in lineas)
        {
            // Buscamos si la línea empieza con el ID que nos pasaron
            if (linea.StartsWith(id.ToString()))
            {
                return MapearDesdeLinea(linea);
            }
        }

        return null; // Si termina de leer todo y no lo encuentra, devuelve null
    }

public void Modificar(Expediente expediente)
    {
        if (!File.Exists(_archivo)) return;

        string[] lineas = File.ReadAllLines(_archivo);
        var lineasActualizadas = new List<string>();

        foreach (var linea in lineas)
        {
            if (linea.StartsWith(expediente.Id.ToString()))
            {
                // Si es el expediente a modificar, armamos la nueva línea con los datos actualizados
                string nuevaLinea = $"{expediente.Id}|{expediente.Caratula.Valor}|{expediente.Estado}|{expediente.FechaCreacion}|{expediente.FechaUltimaModificacion}|{expediente.UsuarioUltimoCambio}";
                lineasActualizadas.Add(nuevaLinea);
            }
            else
            {
                // Si es otro expediente, dejamos la línea como estaba
                lineasActualizadas.Add(linea);
            }
        }

        // Sobrescribimos el archivo completo con las líneas actualizadas
        File.WriteAllLines(_archivo, lineasActualizadas);
    }

    public void Eliminar(Guid id)
    {
        if (!File.Exists(_archivo)) return;

        string[] lineas = File.ReadAllLines(_archivo);
        var lineasActualizadas = new List<string>();

        foreach (var linea in lineas)
        {
            // Solo guardamos en la nueva lista las líneas que NO sean la del expediente a borrar
            if (!linea.StartsWith(id.ToString()))
            {
                lineasActualizadas.Add(linea);
            }
        }

        // Sobrescribimos el archivo (básicamente, guardamos todo menos el borrado)
        File.WriteAllLines(_archivo, lineasActualizadas);
    }

    private Expediente MapearDesdeLinea(string linea)
    {
        string[] campos = linea.Split('|');

        Guid id = Guid.Parse(campos[0]);
        Caratula caratula = new Caratula(campos[1]); 
        EstadoEnum estado = Enum.Parse<EstadoEnum>(campos[2]); 
        DateTime fechaCreacion = DateTime.Parse(campos[3]);
        DateTime fechaModificacion = DateTime.Parse(campos[4]);
        Guid usuarioId = Guid.Parse(campos[5]);

        // Usamos el Factory Method con el orden que definiste en tu constructor privado
        return Expediente.Reconstruir(id, fechaCreacion, fechaModificacion, usuarioId, caratula, estado);
    }
}
