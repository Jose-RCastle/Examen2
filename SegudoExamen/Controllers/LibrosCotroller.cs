using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegudoExamen.Models;
using SegudoExamen.Services;

namespace SegudoExamen.Controllers;

[ApiController]
[Route("api/libros")]
[Authorize]
public class LibrosController : ControllerBase
{
    private readonly FakeFirebaseService _fakeService;

    public LibrosController(FakeFirebaseService fakeService)
    {
        _fakeService = fakeService;
    }

    [HttpPost]
    [Authorize(Roles = "bibliotecario,admin")]
    public async Task<IActionResult> CrearLibro([FromBody] CrearLibroRequest request)
    {
        try
        {
            // Validar ISBN único (simulado)
            var libros = await _fakeService.GetLibros();
            if (libros.Any(l => l.ISBN == request.ISBN))
                return BadRequest(new { mensaje = "El ISBN ya existe en el sistema" });

            // Validar copias
            if (request.CopiasDisponibles > request.CopiasTotal)
                return BadRequest(new { mensaje = "Copias disponibles no pueden ser mayores que el total" });

            var libro = new Libro
            {
                Id = Guid.NewGuid().ToString(),
                Titulo = request.Titulo,
                Autor = request.Autor,
                ISBN = request.ISBN,
                Categoria = request.Categoria,
                Editorial = request.Editorial,
                AnioPublicacion = request.AnioPublicacion,
                CopiasDisponibles = request.CopiasDisponibles,
                CopiasTotal = request.CopiasTotal,
                Ubicacion = request.Ubicacion,
                Estado = request.CopiasDisponibles > 0 ? "disponible" : "agotado",
                Descripcion = request.Descripcion,
                FechaIngreso = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            await _fakeService.AddLibro(libro);

            return Ok(new
            {
                mensaje = "Libro creado exitosamente",
                libroId = libro.Id
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno", error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetLibros(
        [FromQuery] string categoria = null,
        [FromQuery] string autor = null,
        [FromQuery] bool? disponible = null)
    {
        try
        {
            var libros = await _fakeService.GetLibros();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(categoria))
                libros = libros.Where(l => l.Categoria?.Contains(categoria, StringComparison.OrdinalIgnoreCase) == true).ToList();

            if (!string.IsNullOrEmpty(autor))
                libros = libros.Where(l => l.Autor?.Contains(autor, StringComparison.OrdinalIgnoreCase) == true).ToList();

            if (disponible.HasValue)
            {
                if (disponible.Value)
                    libros = libros.Where(l => l.CopiasDisponibles > 0).ToList();
                else
                    libros = libros.Where(l => l.CopiasDisponibles == 0).ToList();
            }

            return Ok(libros);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLibro(string id)
    {
        try
        {
            var libro = await _fakeService.GetLibroById(id);
            if (libro == null)
                return NotFound(new { mensaje = "Libro no encontrado" });

            return Ok(libro);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "bibliotecario,admin")]
    public async Task<IActionResult> ActualizarLibro(string id, [FromBody] ActualizarLibroRequest request)
    {
        try
        {
            var libro = await _fakeService.GetLibroById(id);
            if (libro == null)
                return NotFound(new { mensaje = "Libro no encontrado" });

            // Validación: no reducir copiasTotal si hay préstamos activos
            if (request.CopiasTotal < libro.CopiasTotal)
            {
                int prestadas = libro.CopiasTotal - libro.CopiasDisponibles;
                if (request.CopiasTotal < prestadas)
                    return BadRequest(new { mensaje = "No se pueden eliminar copias que están prestadas" });
            }

            // Actualizar
            libro.Titulo = request.Titulo ?? libro.Titulo;
            libro.Autor = request.Autor ?? libro.Autor;
            libro.Categoria = request.Categoria ?? libro.Categoria;
            libro.Editorial = request.Editorial ?? libro.Editorial;
            libro.AnioPublicacion = request.AnioPublicacion ?? libro.AnioPublicacion;
            libro.CopiasTotal = request.CopiasTotal ?? libro.CopiasTotal;
            libro.CopiasDisponibles = request.CopiasDisponibles ?? libro.CopiasDisponibles;
            libro.Ubicacion = request.Ubicacion ?? libro.Ubicacion;
            libro.Descripcion = request.Descripcion ?? libro.Descripcion;

            // Actualizar estado basado en disponibilidad
            libro.Estado = libro.CopiasDisponibles > 0 ? "disponible" : "agotado";

            await _fakeService.UpdateLibro(libro);

            return Ok(new { mensaje = "Libro actualizado exitosamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "bibliotecario,admin")]
    public async Task<IActionResult> EliminarLibro(string id)
    {
        try
        {
            var libro = await _fakeService.GetLibroById(id);
            if (libro == null)
                return NotFound(new { mensaje = "Libro no encontrado" });

            // Validar que no tenga préstamos activos (simulado)
            bool tienePrestamosActivos = false; // Simular consulta
            bool tieneReservasPendientes = false; // Simular consulta

            if (tienePrestamosActivos)
                return BadRequest(new { mensaje = "No se puede eliminar un libro con préstamos activos" });

            if (tieneReservasPendientes)
                return BadRequest(new { mensaje = "No se puede eliminar un libro con reservas pendientes" });

            await _fakeService.DeleteLibro(id);

            return Ok(new { mensaje = "Libro eliminado exitosamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno", error = ex.Message });
        }
    }
}

// DTOs
public class CrearLibroRequest
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string ISBN { get; set; }
    public string Categoria { get; set; }
    public string Editorial { get; set; }
    public int AnioPublicacion { get; set; }
    public int CopiasDisponibles { get; set; }
    public int CopiasTotal { get; set; }
    public string Ubicacion { get; set; }
    public string Descripcion { get; set; }
}

public class ActualizarLibroRequest
{
    public string? Titulo { get; set; }
    public string? Autor { get; set; }
    public string? Categoria { get; set; }
    public string? Editorial { get; set; }
    public int? AnioPublicacion { get; set; }
    public int? CopiasDisponibles { get; set; }
    public int? CopiasTotal { get; set; }
    public string? Ubicacion { get; set; }
    public string? Descripcion { get; set; }
}