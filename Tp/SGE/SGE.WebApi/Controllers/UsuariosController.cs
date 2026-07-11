using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Registrar([FromBody] RegistrarUsuarioDTO request)
        {
            var response = _registrarUsuarioUseCase.Ejecutar(request);
            return Ok(response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDTO request)
        {
            var response = _loginUseCase.Ejecutar(request);
            return Ok(response);
        }

        [HttpPut("modificar")]
        [Authorize]
        public IActionResult ModificarMisDatos([FromBody] ModificarMisDatosDTO request)
        {
            var idUsuario = ObtenerIdUsuarioDelToken();
            var response = _modificarMisDatosUseCase.Ejecutar(request, idUsuario);
            return Ok(response);
        }

        [HttpPut("permisos")]
        [Authorize]
        public IActionResult CambiarPermisos([FromBody] CambiarPermisosDTO request)
        {
            var idUsuario = ObtenerIdUsuarioDelToken();
            var response = _cambiarPermisosUseCase.Ejecutar(request, idUsuario);
            return Ok(response);
        }

        [HttpGet("consultar")]
        [Authorize]
        public IActionResult Consultar([FromQuery] ConsultarUsuarioDTO request)
        {
            var response = _consultarUsuarioUseCase.Ejecutar(request);
            return Ok(response);
        }

        [HttpDelete("eliminar")]
        [Authorize]
        public IActionResult Eliminar([FromBody] EliminarUsuarioDTO request)
        {
            var idUsuario = ObtenerIdUsuarioDelToken();
            var response = _eliminarUsuarioUseCase.Ejecutar(request, idUsuario);
            return Ok(response);
        }
    }
}