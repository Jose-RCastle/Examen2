using SegundoExamen.DTOs;
using SegundoExamen.Models;

namespace SegundoExamen.Services.Interfaces
{
    public interface IPrestamoService
    {
        Task<Prestamo> CreatePrestamo(string usuarioId, CreatePrestamoDto createPrestamoDto);
        Task<Prestamo> GetPrestamoById(string prestamoId);
        Task<List<Prestamo>> GetPrestamosByUsuarioId(string usuarioId);
        Task<List<Prestamo>> GetAllPrestamos();
        Task<List<Prestamo>> GetVencidosPrestamos();
        Task<Prestamo> DevolverPrestamo(string prestamoId);
        Task<Prestamo> RenovarPrestamo(string prestamoId);
    }
}