using RestWithASPNET.Model.Context;
using RestWithASPNET.Business;
using RestWithASPNET.Business.Implementations;
using Microsoft.EntityFrameworkCore;
using RestWithASPNET.Repository;
using EvolveDb;
using Serilog;
using MySqlConnector;
using RestWithASPNET.Repository.Generic;
using RestWithASPNET.Hypermedia.Filters;
using RestWithASPNET.Hypermedia.Enricher;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;
using RestWithASPNET.Services;
using RestWithASPNET.Services.Implementations;
using RestWithASPNET.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
var appName = "REST API's RESTful From 0 to Azure with ASP .NET Core 8 and Docker";
var appVersion = "v1";
var appDescription = $"REST API RESTful developed in course '{appName}'";

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var tokenConfigurations = new TokenConfiguration();

new ConfigureFromConfigurationOptions<TokenConfiguration>(
    builder.Configuration.GetSection("TokenConfigurations")
).Configure(tokenConfigurations);

builder.Services.AddSingleton(tokenConfigurations);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = tokenConfigurations.Issuer,
        ValidAudience = tokenConfigurations.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
    };
});

builder.Services.AddAuthorization(auth => 
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser().Build());
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc(appVersion,
    new OpenApiInfo {
        Title = appName,
        Version = appVersion,
        Description = appDescription,
        Contact = new OpenApiContact{
            Name = "Higor Ribeiro Araujo",
            Url = new Uri("https://github.com/higorribeiro001/RESTWishASP")
        }
    });
});

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

var filterOptions = new HyperMediaFilterOptions();
filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
filterOptions.ContentResponseEnricherList.Add(new BookEnricher());

builder.Services.AddSingleton(filterOptions);

// versioning API
builder.Services.AddApiVersioning();

// dependency inject
builder.Services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();

builder.Services.AddScoped<IBookBusiness, BookBusinessImplementation>();

builder.Services.AddScoped<ILoginBusiness, LoginBusinessImplementation>();

builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger(); // gera o json da aplicação

app.UseSwaggerUI(c => {
    c.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        $"{appName} - {appVersion}"
    );
}); // gera a pagina HTML

var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);

app.UseAuthentication();

app.MapControllers();
app.MapControllerRoute("DefaultApi", "{controller=values}/v{version=apiVersion}/{id?}");

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
