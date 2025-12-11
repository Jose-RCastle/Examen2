using Google.Cloud.Firestore;

namespace SegundoExamen.Models
{
    [FirestoreData]
    public class Prestamo
    {
        [FirestoreProperty]
        public string Id { get; set; } = string.Empty;

        [FirestoreProperty]
        public string UsuarioId { get; set; } = string.Empty;

        [FirestoreProperty]
        public string LibroId { get; set; } = string.Empty;

        [FirestoreProperty]
        public DateTime FechaPrestamo { get; set; } = DateTime.UtcNow;

        [FirestoreProperty]
        public DateTime FechaDevolucionEsperada { get; set; }

        [FirestoreProperty]
        public DateTime? FechaDevolucionReal { get; set; }

        [FirestoreProperty]
        public int DiasRetraso { get; set; } = 0;

        [FirestoreProperty]
        public decimal MultaGenerada { get; set; } = 0;

        [FirestoreProperty]
        public string Estado { get; set; } = "activo";

        [FirestoreProperty]
        public int Renovaciones { get; set; } = 0;
    }
}