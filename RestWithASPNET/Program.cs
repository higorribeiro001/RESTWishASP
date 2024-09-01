using RestWithASPNET.Model.Context;
using RestWithASPNET.Business;
using RestWithASPNET.Business.Implementations;
using Microsoft.EntityFrameworkCore;
using RestWithASPNET.Repository;
using EvolveDb;
using Serilog;
using MySqlConnector;
using RestWithASPNET.Repository.Generic;
using System.Net.Http.Headers;

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


// procurar depois como resolver
// builder.Services.AddMvc(options => 
// {
//     options.RespectBrowserAcceptHeader = true; // para aceitar o que vier setado no cabeçalho da request
//     options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
//     options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
// })
// .AddXmlSerializerFormatters();

// versioning API
builder.Services.AddApiVersioning();

// dependency inject
builder.Services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();

builder.Services.AddScoped<IBookBusiness, BookBusinessImplementation>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

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
