using Google.Cloud.Firestore;

namespace SegundoExamen.Models
{
    [FirestoreData]
    public class Reserva
    {
        [FirestoreProperty]
        public string Id { get; set; } = string.Empty;

        [FirestoreProperty]
        public string UsuarioId { get; set; } = string.Empty;

        [FirestoreProperty]
        public string LibroId { get; set; } = string.Empty;

        [FirestoreProperty]
        public DateTime FechaReserva { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public string Estado { get; set; } = "pendiente";

        [FirestoreProperty]
        public DateTime? FechaNotificacion { get; set; }

        [FirestoreProperty]
        public DateTime FechaExpiracion { get; set; }

        [FirestoreProperty]
        public int Prioridad { get; set; } = 0;
    }
}