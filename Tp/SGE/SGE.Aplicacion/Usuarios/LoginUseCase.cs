using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;

public class LoginUseCase(IUsuarioRepository repo, ITokenProvider tokenProvider)
{
    public LoginResponse Ejecutar(LoginRequest req)
    {
        Usuario? user=repo.ObtenerPorEmail(req.Email);
        if(user == null || ContrasenaUtil.Comparar(req.Contrasena,user.ContrasenaHash))
        {
            throw new AutorizacionException("El mail o la contraseña son incorectos.");
        }

        var token= tokenProvider.GenerarToken(user);
        return new LoginResponse(token)
    }
}