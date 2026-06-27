using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGE.Aplicacion.Expedientes.UseCases;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;

namespace SGE.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpedientesController : ControllerBase
    {
        private readonly AgregarExpedienteUseCase _agregarExpedienteUseCase;
        private readonly ListaExpedientesUseCase _listaExpedientesUseCase;
        private readonly ConsultarExpedienteUseCase _consultarExpedienteUseCase;
        private readonly ModificarCaratulaUseCase _modificarCaratulaUseCase;
        private readonly CambiarEstadoExpediente _cambiarEstadoExpediente;
        private readonly EliminarExpedienteUseCase _eliminarExpedienteUseCase;

        public ExpedientesController(
            AgregarExpedienteUseCase agregarExpedienteUseCase,
            ListaExpedientesUseCase listaExpedientesUseCase,
            ConsultarExpedienteUseCase consultarExpedienteUseCase,
            ModificarCaratulaUseCase modificarCaratulaUseCase,
            CambiarEstadoExpediente cambiarEstadoExpediente,
            EliminarExpedienteUseCase eliminarExpedienteUseCase)
        {
            _agregarExpedienteUseCase = agregarExpedienteUseCase;
            _listaExpedientesUseCase = listaExpedientesUseCase;
            _consultarExpedienteUseCase = consultarExpedienteUseCase;
            _modificarCaratulaUseCase = modificarCaratulaUseCase;
            _cambiarEstadoExpediente = cambiarEstadoExpediente;
            _eliminarExpedienteUseCase = eliminarExpedienteUseCase;
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

        [HttpPost]
        public IActionResult Agregar([FromBody] AgregarExpedienteRequest request)
        {
            try
            {
                var idUsuario = ObtenerIdUsuarioDelToken();
                var response = _agregarExpedienteUseCase.Ejecutar(request, idUsuario);
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
                return StatusCode(500, new { error = "Error inesperado", detalle = ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Listar([FromQuery] ListarExpedientesRequest request)
        {
            try
            {
                var response = _listaExpedientesUseCase.Ejecutar(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al listar expedientes", detalle = ex.Message });
            }
        }

        [HttpGet("consultar")]
        [AllowAnonymous]
        public IActionResult Consultar([FromQuery] ConsultarExpedienteRequest request)
        {
            try
            {
                var response = _consultarExpedienteUseCase.Ejecutar(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPut("caratula")]
        public IActionResult ModificarCaratula([FromBody] ModificarCaratulaRequest request)
        {
            try
            {
                var idUsuario = ObtenerIdUsuarioDelToken();
                var response = _modificarCaratulaUseCase.Ejecutar(request, idUsuario);
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
                return StatusCode(500, new { error = "Error inesperado", detalle = ex.Message });
            }
        }

        [HttpPut("estado")]
        public IActionResult CambiarEstado([FromBody] CambiarEstadoExpRequest request)
        {
            try
            {
                var idUsuario = ObtenerIdUsuarioDelToken();
                var response = _cambiarEstadoExpediente.Ejecutar(request, idUsuario);
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
                return StatusCode(500, new { error = "Error inesperado", detalle = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult Eliminar([FromBody] EliminarExpedienteRequest request)
        {
            try
            {
                var idUsuario = ObtenerIdUsuarioDelToken();
                var response = _eliminarExpedienteUseCase.Ejecutar(request, idUsuario);
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
                return StatusCode(500, new { error = "Error inesperado", detalle = ex.Message });
            }
        }
    }
}