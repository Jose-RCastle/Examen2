using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SegudoExamen.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICIO FAKE (en lugar de Firebase)
builder.Services.AddSingleton<FakeFirebaseService>();
builder.Services.AddHostedService<InitializeFakeDataService>(); // Opcional

// 2. JWT (igual que antes)
var jwtKey = "MI_CLAVE_SUPER_SECRETA_DE_32_CARACTERES_LARGOS!12345";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Servicio para inicializar datos
public class InitializeFakeDataService : IHostedService
{
    private readonly FakeFirebaseService _fakeService;

    public InitializeFakeDataService(FakeFirebaseService fakeService)
    {
        _fakeService = fakeService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _fakeService.InitializeTestData();
        Console.WriteLine("✅ Datos de prueba inicializados");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}