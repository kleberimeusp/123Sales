using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sales.API.Middlewares.JWT;
using Sales.Application.Events;
using Sales.Application.Services;
using Sales.Domain.Interfaces;
using Sales.Infrastructure.Data;
using Sales.Infrastructure.Events;
using Sales.Infrastructure.Repositories;
using Sales.Infrastructure.Services;
using Serilog;
using ISaleRepository = Sales.Infrastructure.Repositories.ISaleRepository;



var builder = WebApplication.CreateBuilder(args);


// ✅ Configure Serilog in the Correct Place
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day));



// ✅ Configuração do banco de dados (Usando InMemory, mas pode ser SQL Server ou outro)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("SalesDB"));

// ✅ Adicionando serviços ao container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ Configuração do Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sales API",
        Version = "v1",
        Description = "API para gerenciar vendas",
        Contact = new OpenApiContact
        {
            Name = "Suporte Sales API",
            Email = "suporte@salesapi.com",
            Url = new Uri("https://salesapi.com")
        }
    });

    // ✅ Configuração do Swagger para JWT Authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu_token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ✅ Configurando CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ✅ Configurando JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new InvalidOperationException("Configurações de JWT ausentes no appsettings.json.");
}

object value = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = issuer,
            ValidAudience = audience
        };
    });

builder.Services.AddAuthorization();

// ✅ Registrando repositórios e serviços
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddSingleton<IEventPublisher, EventPublisher>();

// ✅ Registrando JwtService corretamente
builder.Services.AddSingleton<IJwtService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    return new JwtService(
        config["Jwt:SecretKey"],
        config["Jwt:Issuer"],
        config["Jwt:Audience"]
    );
});

// ✅ Criando a aplicação
var app = builder.Build();

// ✅ Configuração do pipeline de requisições
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales API v1");
        c.RoutePrefix = string.Empty; // Deixa o Swagger acessível na raiz do servidor
    });
}

// ✅ Aplicando segurança
app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Aplicando CORS
app.UseMiddleware<JwtMiddleware>(); // Middleware JWT antes da autenticação
app.UseAuthentication(); // 🔹 Authentication antes da autorização
app.UseAuthorization();

// ✅ Mapeando os endpoints dos Controllers
app.MapControllers();

app.Run();

// ✅ Make `Program` partial to allow reference in tests
public partial class Program { }
