using System;
using SGE.Infraestructura.Repositorios;
using SGE.Infraestructura.Servicios; 
using SGE.Aplicacion.Expedientes.UseCases;
using SGE.Aplicacion.Expedientes.DTOs; 
using SGE.Aplicacion.Tramites.UseCases;
using SGE.Aplicacion.Tramites.DTOs; 
using SGE.Aplicacion.Servicios;
using SGE.Dominio.Tramites; 

// ==========================================
// 1. INYECCIÓN DE DEPENDENCIAS 
// ==========================================

var expedienteRepo = new ExpedienteTxtRepository();
var tramiteRepo = new TramiteTxtRepository();
var autorizacionService = new AutorizacionProvisionalService(); 

var actualizadorEstado = new ActualizadorEstadoExpedienteService(expedienteRepo);

var agregarExpedienteUC = new AgregarExpedienteUseCase(expedienteRepo);
var agregarTramiteUC = new AgregarTramiteUseCase(tramiteRepo, actualizadorEstado);
var eliminarTramiteUC = new EliminarTramiteUseCase(tramiteRepo, autorizacionService, actualizadorEstado);

// ==========================================
// 2. BUCLE PRINCIPAL 
// ==========================================

bool salir = false;

// Simulamos un ID de usuario fijo para el TP
Guid usuarioLogueado = Guid.Parse("11111111-1111-1111-1111-111111111111");

while (!salir)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("==================================================");
    Console.WriteLine("        SISTEMA DE GESTIÓN DE EXPEDIENTES         ");
    Console.WriteLine("==================================================");
    Console.ResetColor();
    Console.WriteLine(" 1. Agregar un nuevo Expediente");
    Console.WriteLine(" 2. Agregar un Trámite a un Expediente");
    Console.WriteLine(" 3. Eliminar un Trámite");
    Console.WriteLine(" 0. Salir del Sistema");
    Console.WriteLine("==================================================");
    Console.Write("Ingrese el número de la opción deseada: ");

    var opcion = Console.ReadLine();

    Console.WriteLine();

    try
    {
        switch (opcion)
        {
            case "1":
                Console.WriteLine("--- ALTA DE EXPEDIENTE ---");
                Console.Write("Ingrese la carátula del expediente: ");
                string caratula = Console.ReadLine() ?? "";

                var requestExp = new AgregarExpedienteRequest(caratula, usuarioLogueado);
                var responseExp = agregarExpedienteUC.Ejecutar(requestExp);
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n¡Expediente creado con éxito! ID: {responseExp.Id}");
                Console.ResetColor();
                break;

            case "2":
                Console.WriteLine("--- ALTA DE TRÁMITE ---");
                Console.Write("Ingrese el ID del Expediente (ej. xxxxxxxx-xxxx-...): ");
                Guid expId = Guid.Parse(Console.ReadLine() ?? Guid.Empty.ToString());
                
                Console.Write("Ingrese el contenido del trámite: ");
                string contenido = Console.ReadLine() ?? "";

                // Respetamos el orden de tu record: (Guid, EtiquetaEnum, string, Guid)
                var requestTramite = new AgregarTramiteRequest(expId, EtiquetaEnum.Resolucion, contenido, usuarioLogueado);
                agregarTramiteUC.Ejecutar(requestTramite);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n¡Trámite agregado con éxito!");
                Console.ResetColor();
                break;

            case "3":
                Console.WriteLine("--- ELIMINAR TRÁMITE ---");
                Console.Write("Ingrese el ID del Trámite a eliminar: ");
                Guid tramiteId = Guid.Parse(Console.ReadLine() ?? Guid.Empty.ToString());

                var requestEliminar = new EliminarTramiteRequest(tramiteId, usuarioLogueado);
                eliminarTramiteUC.Ejecutar(requestEliminar);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nEl trámite fue eliminado del sistema.");
                Console.ResetColor();
                break;

            case "0":
                salir = true;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Saliendo del sistema... ¡Éxitos con la entrega!");
                Console.ResetColor();
                break;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opción no válida. Por favor, ingrese un número del menú.");
                Console.ResetColor();
                break;
        }
    }
    catch (FormatException)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nERROR: El formato del ID ingresado no es válido. Debe ser un GUID (ej: 12345678-1234-1234-1234-123456789abc).");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nERROR: {ex.Message}");
        Console.ResetColor();
    }

    if (!salir)
    {
        Console.WriteLine("\nPresione cualquier tecla para volver al menú principal...");
        Console.ReadKey();
    }
}