using RestWithASPNET.Model.Context;
using RestWithASPNET.Business;
using RestWithASPNET.Business.Implementations;
using Microsoft.EntityFrameworkCore;
using RestWithASPNET.Repository;
using RestWithASPNET.Repository.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(
    connection,
    new MySqlServerVersion(new Version(8, 0, 2)) // verificar a versão do pomelo no restwithaspnet.csproj
));

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
