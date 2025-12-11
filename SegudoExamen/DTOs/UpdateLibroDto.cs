using System.ComponentModel.DataAnnotations;

namespace SegundoExamen.DTOs
{
    public class UpdateLibroDto
    {
        public string? Titulo { get; set; }

        public string? Autor { get; set; }

        public string? Categoria { get; set; }

        public string? Editorial { get; set; }

        public int? AnoPublicacion { get; set; }

        public int? CopiasDisponibles { get; set; }

        public string? Ubicacion { get; set; }

        public string? Estado { get; set; }

        public string? Descripcion { get; set; }
    }
}