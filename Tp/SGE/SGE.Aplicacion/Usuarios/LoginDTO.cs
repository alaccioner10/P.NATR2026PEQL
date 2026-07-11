namespace SGE.Aplicacion.Usuarios.DTOs;

public record class LoginDTO(string Email, string Contrasena);

public record class LoginResponseDTO(string Token);