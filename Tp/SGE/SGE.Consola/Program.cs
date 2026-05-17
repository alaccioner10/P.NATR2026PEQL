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

// Arranca con un ID por defecto, pero ahora se puede cambiar desde el menú
Guid usuarioLogueado = Guid.Parse("11111111-1111-1111-1111-111111111111");

while (!salir)
{
    Console.Clear();
    Console.WriteLine("==================================================");
    Console.WriteLine("        SISTEMA DE GESTIÓN DE EXPEDIENTES         ");
    Console.WriteLine($"        Usuario actual: {usuarioLogueado}");
    Console.WriteLine("==================================================");
    Console.ResetColor();
    Console.WriteLine(" 1. Agregar un nuevo Expediente");
    Console.WriteLine(" 2. Agregar un Trámite a un Expediente");
    Console.WriteLine(" 3. Eliminar un Trámite");
    Console.WriteLine(" 4. Iniciar Sesión / Cambiar de Usuario");
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
                
                Console.WriteLine($"\n¡Expediente creado con éxito! ID: {responseExp.Id}");
                break;

            case "2":
                Console.WriteLine("--- ALTA DE TRÁMITE ---");
                Console.Write("Ingrese el ID del Expediente (ej. xxxxxxxx-xxxx-...): ");
                Guid expId = Guid.Parse(Console.ReadLine() ?? Guid.Empty.ToString());
                
                Console.Write("Ingrese el contenido del trámite: ");
                string contenido = Console.ReadLine() ?? "";

                var requestTramite = new AgregarTramiteRequest(expId, EtiquetaEnum.Resolucion, contenido, usuarioLogueado);
                
                // Capturamos el retorno del caso de uso para obtener el ID del trámite creado
                var responseTramite = agregarTramiteUC.Ejecutar(requestTramite);

                Console.WriteLine($"\n¡Trámite agregado con éxito! ID: {responseTramite.Id}");
                break;

            case "3":
                Console.WriteLine("--- ELIMINAR TRÁMITE ---");
                Console.Write("Ingrese el ID del Trámite a eliminar: ");
                Guid tramiteId = Guid.Parse(Console.ReadLine() ?? Guid.Empty.ToString());

                var requestEliminar = new EliminarTramiteRequest(tramiteId, usuarioLogueado);
                eliminarTramiteUC.Ejecutar(requestEliminar);

                Console.WriteLine("\nEl trámite fue eliminado del sistema.");
                break;

            case "4":
                Console.WriteLine("--- INICIAR SESIÓN / CAMBIAR USUARIO ---");
                Console.Write("Ingrese el ID del Usuario (Formato xxxxxxxx-xxxx-...): ");
                string inputUser = Console.ReadLine() ?? "";
                
                // Parseamos el nuevo GUID introducido por el usuario
                usuarioLogueado = Guid.Parse(inputUser);
                
                Console.WriteLine($"\n¡Sesión iniciada con éxito! Bienvenido.");
                break;

            case "0":
                salir = true;
                Console.WriteLine("Saliendo del sistema... ¡Éxitos con la entrega!");
                break;

            default:
                Console.WriteLine("Opción no válida. Por favor, ingrese un número del menú.");
                break;
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("\nERROR: El formato del ID ingresado no es válido. Debe ser un GUID (ej: 12345678-1234-1234-1234-123456789abc).");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nERROR: {ex.Message}");
    }

    if (!salir)
    {
        Console.WriteLine("\nPresione cualquier tecla para volver al menú principal...");
        Console.ReadKey(); // El freno que mantiene los mensajes en pantalla
    }
}