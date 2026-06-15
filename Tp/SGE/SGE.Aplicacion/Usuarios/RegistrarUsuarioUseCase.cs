using System.Security.Cryptography;
using SGE.Aplicacion.Excepciones;
using SGE.Aplicacion.Usuarios;
using SGE.Aplicacion.Usuarios.DTOs;
using SGE.Dominio.Usuarios;

public class RegistrarUsuarioUseCase
{
    private IUsuarioRepository _userRepo;

    public RegistrarUsuarioUseCase(IUsuarioRepository userRepo)
    {
        _userRepo = userRepo;    
    }

    public RegistrarUsuarioResponse Ejecutar(RegistrarUsuarioRequest req)
    {
        if (_userRepo.ExistePorEmail(req.Email))
        {
            throw new AplicationException("El email ya está registrado");
        }
        Usuario user = new Usuario(req.Nombre,req.Email,ContrasenaUtil.Convertir(req.Contrasena));

        _userRepo.Agregar(user);

        return new RegistrarUsuarioResponse(user.Id,user.Nombre,user.Email,user.EsAdmin,user.Permisos);
    }
}