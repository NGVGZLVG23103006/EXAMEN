using ExamenProgra.Data;
using Microsoft.Data.SqlClient; // Asegúrate de tener este paquete instalado
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<EstudianteRepository>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new EstudianteRepository(connectionString);
});


var app = builder.Build();

// Configura el middleware de manejo de excepciones
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        // Manejo del error de conexión
        context.Items["ErrorMessage"] = "No se pudo conectar a la base de datos. Verifique la cadena de conexión y que el servidor de base de datos esté disponible.";

        // Redirige a la página de error
        context.Response.Redirect("/Error");
    }
});

try
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("Conexión a la base de datos exitosa.");
    }
}
catch (Exception ex)
{
    // Manejo del error de conexión
    Console.WriteLine("No se pudo conectar a la base de datos. Verifique la cadena de conexión y que el servidor de base de datos esté disponible.");
    // Puedes lanzar una excepción o manejar el error de otra manera según tus necesidades
   // throw new InvalidOperationException("No se pudo conectar a la base de datos. Verifique la cadena de conexión y que el servidor de base de datos esté disponible.", ex);
}
finally
{
    // Cierra la conexión si está abierta
    // Esto puede ser opcional ya que se usa un `using` que maneja el cierre
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
