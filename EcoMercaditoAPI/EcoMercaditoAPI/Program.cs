using EcoMercaditoAPI.Concretes.Context;
using EcoMercaditoAPI.Concretes.EcoMercadito;
using EcoMercaditoAPI.Concretes.Usuario;
using EcoMercaditoAPI.Interfaces.EcoMercadito;
using EcoMercaditoAPI.Interfaces.Usuaio;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ==================== SERVICIOS ====================

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Serializar enums como strings (mejor para OpenAPI)
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});


builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IMunicipioService, MunicipioService>();
builder.Services.AddScoped<IAuthentificacion, AuthentificacionService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// ==================== CORS ====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddProblemDetails(options =>
{
    // Personalizar respuestas de error por código de estado
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
    };
});


// ==================== PIPELINE ====================

var app = builder.Build();

// Middleware

// Manejo de errores global
app.UseExceptionHandler();
app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors("PermitirTodo");
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else {
    app.UseCors("PermitirTodo");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
