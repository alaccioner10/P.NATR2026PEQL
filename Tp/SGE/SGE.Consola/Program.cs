using SGE.Infraestructura.Repositorios;
using SGE.Infraestructura.Servicios; 
using SGE.Aplicacion.Expedientes.UseCases;
using SGE.Aplicacion.Expedientes.DTOs; 
using SGE.Aplicacion.Tramites.UseCases;
using SGE.Aplicacion.Tramites.DTOs; 
using SGE.Aplicacion.Servicios;
using SGE.Dominio.Tramites; 
using SGE.Dominio.Expedientes; 

// ==========================================
// 1. INYECCIÓN DE DEPENDENCIAS 
// ==========================================

var expedienteRepo = new ExpedienteTxtRepository();
var tramiteRepo = new TramiteTxtRepository();
var autorizacionService = new AutorizacionProvisionalService(); 

var actualizadorEstado = new ActualizadorEstadoExpedienteService(expedienteRepo);

// Casos de Uso - Expedientes
var agregarExpedienteUC = new AgregarExpedienteUseCase(expedienteRepo);
var consultarExpedienteUC = new ConsultarExpedienteUseCase(expedienteRepo);
var modificarCaratulaUC = new ModificarCaratulaUseCase(expedienteRepo, autorizacionService);
var cambiarEstadoExpUC = new CambiarEstadoExpediente(expedienteRepo, autorizacionService);
var eliminarExpedienteUC = new EliminarExpedienteUseCase(expedienteRepo, tramiteRepo, autorizacionService);

// Casos de Uso - Trámites
var agregarTramiteUC = new AgregarTramiteUseCase(tramiteRepo, actualizadorEstado);
var eliminarTramiteUC = new EliminarTramiteUseCase(tramiteRepo, autorizacionService, actualizadorEstado);
var modificarTramiteUC = new ModificarTramiteUseCase(tramiteRepo, autorizacionService, actualizadorEstado);


// ==========================================
// 2. PANTALLA DE LOGIN OBLIGATORIA
// ==========================================

Console.Clear();
Console.WriteLine("==================================================");
Console.WriteLine("        SISTEMA DE GESTIÓN DE EXPEDIENTES         ");
Console.WriteLine("==================================================");

Guid usuarioLogueado = Guid.Empty;
bool logueado = false;

while (!logueado)
{
    Console.Write("\nPor favor, ingrese su ID de Usuario para iniciar sesión: ");
    string inputUser = Console.ReadLine() ?? "";

    try
    {
        usuarioLogueado = Guid.Parse(inputUser);
        logueado = true;
        
        Console.WriteLine("\n¡Sesión iniciada con éxito!");
        Console.WriteLine("Presione cualquier tecla para ingresar al menú principal...");
        Console.ReadKey();
    }
    catch (FormatException)
    {
        Console.WriteLine("ERROR: El formato no es válido. Debe ser un GUID (ej: 12345678-1234-1234-1234-123456789abc).");
    }
}


// ==========================================
// 3. BUCLE PRINCIPAL (Menú del Sistema)
// ==========================================

bool salir = false;

while (!salir)
{
    Console.Clear();
    Console.WriteLine("==================================================");
    Console.WriteLine("        SISTEMA DE GESTIÓN DE EXPEDIENTES         ");
    Console.WriteLine($"        Usuario actual: {usuarioLogueado}");
    Console.WriteLine("==================================================");
    Console.WriteLine(" 1. Agregar un nuevo Expediente");
    Console.WriteLine(" 2. Consultar Expediente");
    Console.WriteLine(" 3. Modificar Carátula");
    Console.WriteLine(" 4. Modificar Estado de Expediente");
    Console.WriteLine(" 5. Eliminar Expediente");
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine(" 6. Agregar un Trámite a un Expediente");
    Console.WriteLine(" 7. Ver detalle de un Trámite");
    Console.WriteLine(" 8. Modificar un Trámite");
    Console.WriteLine(" 9. Eliminar un Trámite");
    Console.WriteLine("--------------------------------------------------");
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
                Console.WriteLine("--- CONSULTAR EXPEDIENTE ---");
                Console.Write("Ingrese el ID del Expediente a buscar: ");
                Guid expIdConsultar = Guid.Parse(Console.ReadLine() ?? Guid.Empty.ToString());

                var requestConsultarExp = new ConsultarExpedienteRequest(expIdConsultar);
                var responseConsultarExp = consultarExpedienteUC.Ejecutar(requestConsultarExp);

                Console.WriteLine("\nDATOS DEL EXPEDIENTE:");
                Console.WriteLine($"- ID: {responseConsultarExp.Id}");
                Console.WriteLine($"- Carátula: {responseConsultarExp.Caratula}");
                Console.WriteLine($"- Estado: {responseConsultarExp.Estado}");
                Console.WriteLine($"- Fecha Creación: {responseConsultarExp.FechaCreacion}");
                Console.WriteLine($"- Última Modificación: {responseConsultarExp.FechaModificacion}");
                break;

            case "3":
                Console.WriteLine("--- MODIFICAR CARÁTULA ---");
                Console.Write("Ingrese el ID del Expediente a modificar: ");
                Guid expIdModificar = Guid.Parse(Console.ReadLine() ?? Guid.Empty.ToString());
                
                Console.Write("Ingrese la nueva carátula: ");
                string nuevaCaratula = Console.ReadLine() ?? "";

                var requestModificarCaratula = new ModificarCaratulaRequest(expIdModificar, nuevaCaratula, usuarioLogueado, DateTime.Now);
                modificarCaratulaUC.Ejecutar(requestModificarCaratula);

                Console.WriteLine("\n¡Carátula modificada con éxito!");
                break;

            case "4":
Console.WriteLine("--- MODIFICAR ESTADO DE EXPEDIENTE ---");
                Console.Write("Ingrese el ID del Expediente a modificar: ");
                Guid expIdEstado = Guid.Parse(Console.ReadLine() ?? Guid.Empty.ToString());
                
                Console.WriteLine("\nSeleccione el nuevo estado:");
                Console.WriteLine(" 1. Recien Iniciado");
                Console.WriteLine(" 2. Para Resolver");
                Console.WriteLine(" 3. Con Resolucion");
                Console.WriteLine(" 4. En Notificacion");
                Console.WriteLine(" 5. Finalizado");
                Console.Write("\nIngrese el número de la opción: ");
                
                var subOpcion = Console.ReadLine();
                EstadoEnum? nuevoEstado = null;

                switch (subOpcion)
                {
                    case "1":
                        nuevoEstado = EstadoEnum.RecienIniciado;
                        break;
                    case "2":
                        nuevoEstado = EstadoEnum.ParaResolver;
                        break;
                    case "3":
                        nuevoEstado = EstadoEnum.ConResolucion;
                        break;
                    case "4":
                        nuevoEstado = EstadoEnum.EnNotificacion;
                        break;
                    case "5":
                        nuevoEstado = EstadoEnum.Finalizado;
                        break;
                    default:
                        Console.WriteLine("\nERROR: Opción de estado no válida.");
                        break;
                }

                if (nuevoEstado != null)
                {
                    var requestEstado = new CambiarEstadoExpRequest(expIdEstado, usuarioLogueado, nuevoEstado.Value);
                    cambiarEstadoExpUC.Ejecutar(requestEstado);

                    Console.WriteLine($"\n¡Estado modificado a '{nuevoEstado}' con éxito!");
                }
                break;

            case "5":
                Console.WriteLine("--- ELIMINAR EXPEDIENTE ---");
                Console.Write("Ingrese el ID del Expediente a eliminar: ");
                Guid expedienteIdEliminar = Guid.Parse(Console.ReadLine() ?? Guid.Empty.ToString());

                var requestEliminarExp = new EliminarExpedienteRequest(expedienteIdEliminar, usuarioLogueado);
                eliminarExpedienteUC.Ejecutar(requestEliminarExp);

                Console.WriteLine("\nEl expediente fue eliminado del sistema.");
                break;

            case "6":
                Console.WriteLine("--- ALTA DE TRÁMITE ---");
                Console.Write("Ingrese el ID del Expediente (ej. xxxxxxxx-xxxx-...): ");
                Guid expId = Guid.Parse(Console.ReadLine() ?? Guid.Empty.ToString());
                
                Console.Write("Ingrese el contenido del trámite: ");
                string contenido = Console.ReadLine() ?? "";

                var requestTramite = new AgregarTramiteRequest(expId, EtiquetaEnum.Resolucion, contenido, usuarioLogueado);
                var responseTramite = agregarTramiteUC.Ejecutar(requestTramite);

                Console.WriteLine($"\n¡Trámite agregado con éxito! ID: {responseTramite.Id}");
                break;

            case "7":
                Console.WriteLine("--- VER DETALLE DE TRÁMITE ---");
                Console.Write("Ingrese el ID del Trámite que desea buscar: ");
                Guid tramiteIdBuscar = Guid.Parse(Console.ReadLine() ?? Guid.Empty.ToString());

                var tramiteEncontrado = tramiteRepo.ObtenerPorId(tramiteIdBuscar);

                if (tramiteEncontrado != null)
                {
                    Console.WriteLine("\nDATOS DEL TRÁMITE:");
                    Console.WriteLine($"- ID Trámite: {tramiteEncontrado.Id}");
                    Console.WriteLine($"- ID Expediente: {tramiteEncontrado.ExpedienteId}");
                    Console.WriteLine($"- Etiqueta (Estado): {tramiteEncontrado.Etiqueta}");
                    Console.WriteLine($"- Contenido: {tramiteEncontrado.Contenido.Valor}");
                    Console.WriteLine($"- Fecha Creación: {tramiteEncontrado.FechaCreacion}");
                }
                else
                {
                    Console.WriteLine("\nNo se encontró ningún trámite con ese ID.");
                }
                break;

            case "8":
                Console.WriteLine("--- MODIFICAR TRÁMITE ---");
                Console.Write("Ingrese el ID del Trámite a modificar: ");
                Guid tramiteIdModificar = Guid.Parse(Console.ReadLine() ?? Guid.Empty.ToString());
                
                Console.Write("Ingrese el nuevo contenido del trámite: ");
                string nuevoContenido = Console.ReadLine() ?? "";

                var requestModificar = new ModificarTramiteRequest(tramiteIdModificar, nuevoContenido, usuarioLogueado);
                modificarTramiteUC.Ejecutar(requestModificar);

                Console.WriteLine("\n¡Trámite modificado con éxito!");
                break;

            case "9":
                Console.WriteLine("--- ELIMINAR TRÁMITE ---");
                Console.Write("Ingrese el ID del Trámite a eliminar: ");
                Guid tramiteIdEliminar = Guid.Parse(Console.ReadLine() ?? Guid.Empty.ToString());

                var requestEliminar = new EliminarTramiteRequest(tramiteIdEliminar, usuarioLogueado);
                eliminarTramiteUC.Ejecutar(requestEliminar);

                Console.WriteLine("\nEl trámite fue eliminado del sistema.");
                break;

            case "0":
                salir = true;
                Console.WriteLine("Saliendo del sistema...");
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
        Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
        Console.ReadKey();
    }
}