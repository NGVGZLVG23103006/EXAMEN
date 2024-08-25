using ExamenProgra.Data;
using Microsoft.Data.SqlClient; // Aseg�rate de tener este paquete instalado
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
        // Manejo del error de conexi�n
        context.Items["ErrorMessage"] = "No se pudo conectar a la base de datos. Verifique la cadena de conexi�n y que el servidor de base de datos est� disponible.";

        // Redirige a la p�gina de error
        context.Response.Redirect("/Error");
    }
});

try
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("Conexi�n a la base de datos exitosa.");
    }
}
catch (Exception ex)
{
    // Manejo del error de conexi�n
    Console.WriteLine("No se pudo conectar a la base de datos. Verifique la cadena de conexi�n y que el servidor de base de datos est� disponible.");
    // Puedes lanzar una excepci�n o manejar el error de otra manera seg�n tus necesidades
   // throw new InvalidOperationException("No se pudo conectar a la base de datos. Verifique la cadena de conexi�n y que el servidor de base de datos est� disponible.", ex);
}
finally
{
    // Cierra la conexi�n si est� abierta
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
