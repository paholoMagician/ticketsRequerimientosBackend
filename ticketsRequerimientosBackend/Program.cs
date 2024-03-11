using Microsoft.EntityFrameworkCore;
using ticketsRequerimientosBackend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using ticketsRequerimientosBackend;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CMSSoftwarecontrolContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.RequireHttpsMetadata = false;
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,//no valida del lado del servidor
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


builder.Services.AddAuthorization(policies =>
{
    policies.AddPolicy("User", p =>
    {
        p.RequireClaim(ClaimTypes.Role, "user");
    });
    policies.AddPolicy("Admin", p =>
    {
        p.RequireClaim(ClaimTypes.Role, "admin");
    });
    policies.AddPolicy("All", p =>
    {
        p.RequireClaim(ClaimTypes.Role, "admin", "user");
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c => {
    //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    //c.IgnoreObsoleteActions();
    //c.IgnoreObsoleteProperties();
    //c.CustomSchemaIds(type => type.FullName);
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authentication",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Autorizacion con Bearer Token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type= ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



//builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
//IServiceCollection serviceCollection = builder.Services.AddDbContext<CMSSoftwarecontrolContext>(opt => opt.UseInMemoryDatabase(databaseName: "CMS-Software-control"));

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

app.UseCors(builder => builder.WithOrigins("http://localhost:4200/", "http://192.168.100.148:2251")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials());

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(120)
};


webSocketOptions.AllowedOrigins.Add("http://192.168.100.148:2251");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseWebSockets(webSocketOptions);
// Agrega autenticación JWT
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

app.MapHub<TicketResolucionHUB>("/hubs/TicketResolucionHub");

app.Run();
