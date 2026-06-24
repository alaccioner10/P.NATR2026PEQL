using Microsoft.AspNetCore.Mvc;
using SGE.Aplicacion.Usuarios.UseCases;
using SGE.Aplicacion.Usuarios.DTOs; // <-- Para usar RegistrarUsuarioRequest y sus respuestas
using SGE.Aplicacion.Autorizacion;  // <-- Por si salta AutorizacionException
using SGE.Aplicacion.Excepciones;   // <-- Por si salta AplicationException

namespace SGE.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly RegistrarUsuarioUseCase _registrarUsuarioUseCase;
        private readonly LoginUseCase _loginUseCase;

        public UsuariosController(
            RegistrarUsuarioUseCase registrarUsuarioUseCase,
            LoginUseCase loginUseCase)
        {
            _registrarUsuarioUseCase = registrarUsuarioUseCase;
            _loginUseCase = loginUseCase;
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
    }
}