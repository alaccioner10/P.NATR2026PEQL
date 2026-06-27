using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Usuarios;
using SGE.Aplicacion.Usuarios.DTOs;
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
        private readonly CambiarPermisosUseCase _cambiarPermisosUseCase;
        private readonly ConsultarUsuarioUseCase _consultarUsuarioUseCase;
        private readonly EliminarUsuarioUseCase _eliminarUsuarioUseCase;

        public UsuariosController(
            RegistrarUsuarioUseCase registrarUsuarioUseCase,
            LoginUseCase loginUseCase,
            ModificarMisDatosUseCase modificarMisDatosUseCase,
            CambiarPermisosUseCase cambiarPermisosUseCase,
            ConsultarUsuarioUseCase consultarUsuarioUseCase,
            EliminarUsuarioUseCase eliminarUsuarioUseCase)
        {
            _registrarUsuarioUseCase = registrarUsuarioUseCase;
            _loginUseCase = loginUseCase;
            _modificarMisDatosUseCase = modificarMisDatosUseCase;
            _cambiarPermisosUseCase = cambiarPermisosUseCase;
            _consultarUsuarioUseCase = consultarUsuarioUseCase;
            _eliminarUsuarioUseCase = eliminarUsuarioUseCase;
        }

        private Guid ObtenerIdUsuarioDelToken()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                throw new UnauthorizedAccessException("No se pudo obtener el ID del usuario del token.");
            }
            return Guid.Parse(claim.Value);
        }

        [HttpPost("registrar")]
        [AllowAnonymous]
        public IActionResult Registrar([FromBody] RegistrarUsuarioRequest request)
        {
            try
            {
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

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
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

        [HttpPut("modificar")]
        [Authorize]
        public IActionResult ModificarMisDatos([FromBody] ModificarMisDatosRequest request)
        {
            try
            {
                var idUsuario = ObtenerIdUsuarioDelToken();
                var response = _modificarMisDatosUseCase.Ejecutar(request, idUsuario);
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

        [HttpPut("permisos")]
        [Authorize]
        public IActionResult CambiarPermisos([FromBody] CambiarPermisosRequest request)
        {
            try
            {
                var idUsuario = ObtenerIdUsuarioDelToken();
                var response = _cambiarPermisosUseCase.Ejecutar(request, idUsuario);
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

        [HttpGet("consultar")]
        [Authorize]
        public IActionResult Consultar([FromQuery] ConsultarUsuarioRequest request)
        {
            try
            {
                var response = _consultarUsuarioUseCase.Ejecutar(request);
                return Ok(response);
            }
            catch (AplicationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado.", detalle = ex.Message });
            }
        }

        [HttpDelete("eliminar")]
        [Authorize]
        public IActionResult Eliminar([FromBody] EliminarUsuarioRequest request)
        {
            try
            {
                var idUsuario = ObtenerIdUsuarioDelToken();
                var response = _eliminarUsuarioUseCase.Ejecutar(request, idUsuario);
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