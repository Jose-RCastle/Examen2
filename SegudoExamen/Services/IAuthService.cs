using SegundoExamen.DTOs;
using SegundoExamen.Models;

namespace SegundoExamen.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterDto registerDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task<Usuario> GetUsuarioById(string usuarioId);
        Task<Usuario> GetUsuarioByCorreo(string correo);
        string GenerateJwtToken(Usuario usuario);
    }
}