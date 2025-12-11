using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SegudoExamen.Models;
using SegudoExamen.Services;

namespace SegudoExamen.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly FakeFirebaseService _fakeService;
    private readonly IConfiguration _config;

    public AuthController(FakeFirebaseService fakeService, IConfiguration config)
    {
        _fakeService = fakeService;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            // Validar que todos los campos requeridos estén
            if (string.IsNullOrEmpty(request.Correo) || string.IsNullOrEmpty(request.Contrasena))
                return BadRequest(new { mensaje = "Correo y contraseña son requeridos" });

            // Verificar correo único
            var usuarioExistente = await _fakeService.GetUsuarioByEmail(request.Correo);
            if (usuarioExistente != null)
                return BadRequest(new { mensaje = "El correo ya está registrado" });

            // Crear usuario
            var usuario = new Usuario
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Correo = request.Correo,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(request.Contrasena),
                Edad = request.Edad,
                NumeroIdentidad = request.NumeroIdentidad,
                Telefono = request.Telefono,
                Rol = "usuario", // Por defecto
                Activo = true,
                FechaRegistro = Timestamp.FromDateTime(DateTime.UtcNow),
                Multas = 0
            };

            await _fakeService.AddUsuario(usuario);

            return Ok(new
            {
                mensaje = "Usuario registrado exitosamente",
                userId = usuario.Id,
                nombre = $"{usuario.Nombre} {usuario.Apellido}"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno", error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            // Buscar usuario por correo
            var usuario = await _fakeService.GetUsuarioByEmail(request.Correo);

            if (usuario == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas" });

            // Verificar cuenta activa
            if (!usuario.Activo)
                return Unauthorized(new { mensaje = "Cuenta inactiva. Contacte al administrador." });

            // Verificar contraseña
            if (!BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.Contrasena))
                return Unauthorized(new { mensaje = "Credenciales inválidas" });

            // Generar JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? "MI_CLAVE_SUPER_SECRETA_DE_32_CARACTERES_LARGOS!12345");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", usuario.Id),
                    new Claim("correo", usuario.Correo),
                    new Claim("rol", usuario.Rol),
                    new Claim("nombre", usuario.Nombre)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                usuario = new
                {
                    id = usuario.Id,
                    nombre = usuario.Nombre,
                    apellido = usuario.Apellido,
                    correo = usuario.Correo,
                    rol = usuario.Rol
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno", error = ex.Message });
        }
    }
}

// DTOs para las requests
public class RegisterRequest
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Correo { get; set; }
    public string Contrasena { get; set; }
    public int Edad { get; set; }
    public string NumeroIdentidad { get; set; }
    public string Telefono { get; set; }
}

public class LoginRequest
{
    public string Correo { get; set; }
    public string Contrasena { get; set; }
}