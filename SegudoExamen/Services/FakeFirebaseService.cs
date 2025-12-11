using SegudoExamen.Models;
using Google.Cloud.Firestore;

namespace SegudoExamen.Services;

public class FakeFirebaseService
{
    // Listas en memoria para simular Firestore
    private List<Usuario> _usuarios = new();
    private List<Libro> _libros = new();
    private List<Prestamo> _prestamos = new();
    private List<Reserva> _reservas = new();

    // Métodos para Usuarios
    public async Task<Usuario> GetUsuarioById(string id)
        => _usuarios.FirstOrDefault(u => u.Id == id);

    public async Task<Usuario> GetUsuarioByEmail(string email)
        => _usuarios.FirstOrDefault(u => u.Correo == email);

    public async Task AddUsuario(Usuario usuario)
    {
        _usuarios.Add(usuario);
        await Task.CompletedTask;
    }

    // Métodos para Libros
    public async Task<List<Libro>> GetLibros()
        => await Task.FromResult(_libros);

    public async Task<Libro> GetLibroById(string id)
        => _libros.FirstOrDefault(l => l.Id == id);

    public async Task AddLibro(Libro libro)
    {
        _libros.Add(libro);
        await Task.CompletedTask;
    }

    public async Task UpdateLibro(Libro libro)
    {
        var index = _libros.FindIndex(l => l.Id == libro.Id);
        if (index != -1) _libros[index] = libro;
        await Task.CompletedTask;
    }

    public async Task DeleteLibro(string id)
    {
        _libros.RemoveAll(l => l.Id == id);
        await Task.CompletedTask;
    }

    // Inicializar con datos de prueba
    public void InitializeTestData()
    {
        // Usuarios de prueba
        _usuarios.Add(new Usuario
        {
            Id = "1",
            Nombre = "Admin",
            Apellido = "Sistema",
            Correo = "admin@biblioteca.com",
            Contrasena = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Rol = "admin",
            Activo = true,
            Multas = 0
        });

        _usuarios.Add(new Usuario
        {
            Id = "2",
            Nombre = "Bibliotecario",
            Apellido = "Principal",
            Correo = "biblio@biblioteca.com",
            Contrasena = BCrypt.Net.BCrypt.HashPassword("biblio123"),
            Rol = "bibliotecario",
            Activo = true,
            Multas = 0
        });

        // Libros de prueba
        _libros.Add(new Libro
        {
            Id = "L1",
            Titulo = "Cien años de soledad",
            Autor = "Gabriel García Márquez",
            ISBN = "9788437604947",
            Categoria = "Novela",
            Editorial = "Sudamericana",
            AnioPublicacion = 1967,
            CopiasDisponibles = 3,
            CopiasTotal = 5,
            Estado = "disponible"
        });
    }
}