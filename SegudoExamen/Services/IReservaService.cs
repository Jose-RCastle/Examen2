using SegundoExamen.DTOs;
using SegundoExamen.Models;

namespace SegundoExamen.Services.Interfaces
{
    public interface IReservaService
    {
        Task<Reserva> CreateReserva(string usuarioId, CreateReservaDto createReservaDto);
        Task<Reserva> GetReservaById(string reservaId);
        Task<List<Reserva>> GetReservasByUsuarioId(string usuarioId);
        Task<List<Reserva>> GetReservasByLibroId(string libroId);
        Task<List<Reserva>> GetAllReservas();
        Task<bool> CancelReserva(string reservaId);
    }
}