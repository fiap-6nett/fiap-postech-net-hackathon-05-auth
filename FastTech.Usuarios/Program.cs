using System.Reflection;
using System.Security.Claims;
using System.Text;
using FastTech.Usuarios.Application.Interfaces;
using FastTech.Usuarios.Application.Services;
using FastTech.Usuarios.Application.Settings;
using FastTech.Usuarios.Contract.CreateClient;
using FastTech.Usuarios.Contract.CreateEmployee;
using FastTech.Usuarios.Contract.DeleteUser;
using FastTech.Usuarios.Contract.GenerateTokens;
using FastTech.Usuarios.Contract.GetUserById;
using FastTech.Usuarios.Contract.UpdateUser;
using FastTech.Usuarios.Infra.Persistense;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserCommandStore, UserCommandStore>();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no campo abaixo. Exemplo: Bearer {seu token}"
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
            new string[] { }
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<IdentitySettings>(builder.Configuration.GetSection("Identity"));

builder.Services.AddValidatorsFromAssemblyContaining<CreateClientCommand>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeCommand>();
builder.Services.AddValidatorsFromAssemblyContaining<DeleteUserCommand>();
builder.Services.AddValidatorsFromAssemblyContaining<TokensCommand>();
builder.Services.AddValidatorsFromAssemblyContaining<GetUserByIdQuery>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserCommand>();

// ⚠️ Leitura segura da SecretKey via variável de ambiente
var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
if (string.IsNullOrEmpty(jwtSecretKey))
    throw new InvalidOperationException("JWT_SECRET_KEY não definida no ambiente.");

// Configuração de autenticação JWT
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.FromMinutes(5),
        ValidIssuer = builder.Configuration["Identity:Issuer"],
        ValidAudience = builder.Configuration["Identity:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Redirecionar a raiz para a UI do Swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html", false);
    return Task.CompletedTask;
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    //db.Database.EnsureDeleted();   // ⚠️ CUIDADO: Apaga o banco todo

    // Força a criação do banco se não existir
    var bancoCriado = db.Database.EnsureCreated();
    Console.WriteLine($"Banco criado? {bancoCriado}");

    // Garante que o usuário admin será criado
    await SeedData.EnsureAdminUserAsync(scope.ServiceProvider);
}

app.Run();
