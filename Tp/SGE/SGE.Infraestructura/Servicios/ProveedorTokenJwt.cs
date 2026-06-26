using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;
using static Microsoft.IdentityModel.Tokens.SecurityAlgorithms;

namespace SGE.Infraestructura.Servicios;

public class ProveedorTokenJwt : ITokenProvider
{
    private readonly string _claveSecreta;

    public ProveedorTokenJwt(string claveSecreta)
    {
        _claveSecreta = claveSecreta;
    }

    public string GenerarToken(Usuario user)
    {
        var manejadorToken = new JwtSecurityTokenHandler();
        var claveEnBytes = Encoding.UTF8.GetBytes(_claveSecreta);

        var reclamos = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Nombre),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("esAdmin", user.EsAdmin.ToString().ToLower())
        };

        if (!user.EsAdmin)
        {
            foreach (var permiso in user.Permisos)
            {
                reclamos.Add(new Claim("permiso", permiso.ToString()));
            }
        }

        var descriptorToken = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(reclamos),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(claveEnBytes),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = manejadorToken.CreateToken(descriptorToken);
        return manejadorToken.WriteToken(token);
    }
}
