using Microsoft.Data.SqlClient;
using Negocio;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


DbConfig.ConnectionString = builder.Configuration.GetConnectionString("CatalogoDB") ?? "";

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/health/db", () =>
{
    try
    {
        using var cnn = new SqlConnection(DbConfig.ConnectionString);
        cnn.Open();
        return Results.Ok("DB OK");
    }
    catch (Exception ex)
    {
        return Results.Problem($"DB ERROR: {ex.Message}");
    }
});

app.Run();
