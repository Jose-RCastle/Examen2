using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly FirestoreDb _firestoreDb;

    public TestController(FirestoreDb firestoreDb)
    {
        _firestoreDb = firestoreDb;
    }

    [HttpGet("firebase")]
    public async Task<IActionResult> TestFirebase()
    {
        try
        {
            // Crear una colección de prueba
            var collection = _firestoreDb.Collection("test");
            var document = collection.Document("testDoc");

            await document.SetAsync(new
            {
                mensaje = "Conexión exitosa",
                fecha = DateTime.UtcNow
            });

            return Ok(new
            {
                success = true,
                message = "Firebase conectado correctamente"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                error = ex.Message,
                detail = ex.InnerException?.Message
            });
        }
    }
}