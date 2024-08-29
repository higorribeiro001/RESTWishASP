using RestWithASPNET.Model.Context;
using RestWithASPNET.Services;
using RestWithASPNET.Services.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(
    connection,
    new MySqlServerVersion(new Version(8, 0, 2)) // verificar a versão do pomelo no restwithaspnet.csproj
));

// dependency inject
builder.Services.AddScoped<IPersonService, PersonServiceImplementation>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();
