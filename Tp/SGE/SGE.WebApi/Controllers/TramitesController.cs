using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Tramites.UseCases;

namespace SGE.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TramitesController : ControllerBase
    {
        private readonly AgregarTramiteUseCase _agregarTramiteUseCase;
        private readonly ModificarTramiteUseCase _modificarTramiteUseCase;
        private readonly EliminarTramiteUseCase _eliminarTramiteUseCase;
        private readonly ListarTramitesPorExpedienteUseCase _listarTramitesUseCase;

        public TramitesController(
            AgregarTramiteUseCase agregarTramiteUseCase,
            ModificarTramiteUseCase modificarTramiteUseCase,
            EliminarTramiteUseCase eliminarTramiteUseCase,
            ListarTramitesPorExpedienteUseCase listarTramitesUseCase)
        {
            _agregarTramiteUseCase = agregarTramiteUseCase;
            _modificarTramiteUseCase = modificarTramiteUseCase;
            _eliminarTramiteUseCase = eliminarTramiteUseCase;
            _listarTramitesUseCase = listarTramitesUseCase;
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
        public IActionResult Agregar([FromBody] AgregarTramiteRequest request)
        {
            try
            {
                var idUsuario = ObtenerIdUsuarioDelToken();
                var response = _agregarTramiteUseCase.Ejecutar(request, idUsuario);
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
                return StatusCode(500, new { error = "Error inesperado al agregar el trámite", detalle = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Modificar([FromBody] ModificarTramiteRequest request)
        {
            try
            {
                var idUsuario = ObtenerIdUsuarioDelToken();
                var response = _modificarTramiteUseCase.Ejecutar(request, idUsuario);
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
                return StatusCode(500, new { error = "Error inesperado al modificar el trámite", detalle = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult Eliminar([FromBody] EliminarTramiteRequest request)
        {
            try
            {
                var idUsuario = ObtenerIdUsuarioDelToken();
                var response = _eliminarTramiteUseCase.Ejecutar(request, idUsuario);
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
                return StatusCode(500, new { error = "Error inesperado al eliminar el trámite", detalle = ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Listar([FromQuery] ListaTramitesPorExpedienteRequest request)
        {
            try
            {
                var response = _listarTramitesUseCase.Ejecutar(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al listar los trámites del expediente", detalle = ex.Message });
            }
        }
    }
}
