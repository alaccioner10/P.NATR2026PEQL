public record class LoginRequest(string Email, string Contrasena);

public record class LoginResponse(string Token);