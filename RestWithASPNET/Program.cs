using RestWithASPNET.Model.Context;
using RestWithASPNET.Business;
using RestWithASPNET.Business.Implementations;
using Microsoft.EntityFrameworkCore;
using RestWithASPNET.Repository;
using RestWithASPNET.Repository.Implementations;
using EvolveDb;
using Serilog;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(
    connection,
    new MySqlServerVersion(new Version(8, 0, 2)) // verificar a versão do pomelo no restwithaspnet.csproj
));

if (builder.Environment.IsDevelopment())
{
    MigrateDatabase(connection);
}

// versioning API
builder.Services.AddApiVersioning();

// dependency inject
builder.Services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
builder.Services.AddScoped<IPersonRepository, PersonRepositoryImplementation>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();

void MigrateDatabase(string? connection)
{
    try
    {
        var evolveConnection = new MySqlConnection(connection);
        var evolve = new Evolve(evolveConnection, Log.Information)
        {
            Locations = new List<string> { "db/migrations", "db/dataset" }, // em quais diretorios estão as nossas migrations
            IsEraseDisabled = true,
        };
        evolve.Migrate();
    } 
    catch (Exception ex) 
    {
        Log.Error("Database migration failed", ex);
        throw;
    }
}
