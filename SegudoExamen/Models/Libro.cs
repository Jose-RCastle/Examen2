using Google.Cloud.Firestore;

namespace SegundoExamen.Models
{
    [FirestoreData]
    public class Libro
    {
        [FirestoreProperty]
        public string Id { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Titulo { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Autor { get; set; } = string.Empty;

        [FirestoreProperty]
        public string ISBN { get; set; } = string.Empty; // Debe ser único

        [FirestoreProperty]
        public string Categoria { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Editorial { get; set; } = string.Empty;

        [FirestoreProperty] public int AnoPublicacion { get; set; } = int.MaxValue;