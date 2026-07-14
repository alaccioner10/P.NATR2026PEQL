using SGE.Aplicacion.Servicios;
using SGE.Aplicacion.Usuarios.DTOs;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class LoginUseCase
{
    private readonly IUsuarioRepository _repo;
    private readonly ITokenProvider _tokenProvider;
    private readonly IHashService _hashService;

    public LoginUseCase(IUsuarioRepository repo, ITokenProvider tokenProvider, IHashService hashService)
    {
        _repo = repo;
        _tokenProvider = tokenProvider;
        _hashService = hashService;
    }

    public LoginResponseDTO Ejecutar(LoginDTO req)
    {
        Usuario? user = _repo.ObtenerPorEmail(req.Email);
        if (user == null || !_hashService.Verify(req.Contrasena, user.ContrasenaHash))
        {
            throw new UnauthorizedAccessException("El mail o la contraseña son incorrectos.");
        }

        var token = _tokenProvider.GenerarToken(user);
        return new LoginResponseDTO(token);
    }
}