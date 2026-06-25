using Microsoft.AspNetCore.Mvc;
using SGE.Aplicacion.Autorizacion;  // <-- Por si salta AutorizacionException
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Usuarios; // <-- Para usar ModificarMisDatosRequest
using SGE.Aplicacion.Usuarios.DTOs; // <-- Para usar RegistrarUsuarioRequest y sus respuestas
using SGE.Aplicacion.Usuarios.UseCases;

namespace SGE.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly RegistrarUsuarioUseCase _registrarUsuarioUseCase;
        private readonly LoginUseCase _loginUseCase;
        private readonly ModificarMisDatosUseCase _modificarMisDatosUseCase;

        public UsuariosController(
            RegistrarUsuarioUseCase registrarUsuarioUseCase,
            LoginUseCase loginUseCase,
            ModificarMisDatosUseCase modificarMisDatosUseCase)
        {
            _registrarUsuarioUseCase = registrarUsuarioUseCase;
            _loginUseCase = loginUseCase;
            _modificarMisDatosUseCase = modificarMisDatosUseCase;
        }

        // POST: api/usuarios/registrar
        [HttpPost("registrar")]
        public IActionResult Registrar([FromBody] RegistrarUsuarioRequest request)
        {
            try
            {
                // Le pasamos tu DTO 'RegistrarUsuarioRequest' tal como lo pide el método
                var response = _registrarUsuarioUseCase.Ejecutar(request);
                return Ok(response);
            }
            catch (AplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado.", detalle = ex.Message });
            }
        }

        // POST: api/usuarios/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                // Tu LoginUseCase ya espera un 'LoginRequest' de SGE.Aplicacion.Usuarios
                var response = _loginUseCase.Ejecutar(request);
                return Ok(response);
            }
            catch (AutorizacionException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado.", detalle = ex.Message });
            }
        }

        // PUT: api/usuarios/modificar/{id}
        [HttpPut("modificar/{id}")]
        public IActionResult ModificarMisDatos([FromRoute] Guid id, [FromBody] ModificarMisDatosRequest request)
        {
            try
            {
                // El caso de uso valida que el id del path coincida con el del body
                var response = _modificarMisDatosUseCase.Ejecutar(request, id);
                return Ok(response);
            }
            catch (AutorizacionException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (AplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado.", detalle = ex.Message });
            }
        }
    }
}