using Microsoft.EntityFrameworkCore;
using ticketsRequerimientosBackend.Models;
using Microsoft.Extensions.Configuration;
using ticketsRequerimientosBackend.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c => {
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
});

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json").Build();

// Agrega JWT authentication
builder.Services.AddJwtAuthentication(configuration);

var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CMSSoftwarecontrolContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
IServiceCollection serviceCollection = builder.Services.AddDbContext<CMSSoftwarecontrolContext>(opt => opt.UseInMemoryDatabase(databaseName: "CMS-Software-control"));

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 1024;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.WithOrigins("https://localhost:7106", "http://192.168.100.141:5106", "http://localhost:4200", "*")
                              .AllowAnyHeader().AllowAnyMethod()
                              .AllowAnyOrigin());

app.UseHttpsRedirection();
app.UseStaticFiles();

// Agrega autenticación JWT
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();
app.Run();
