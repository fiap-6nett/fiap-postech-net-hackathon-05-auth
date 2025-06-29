using FastTech.Usuarios.Domain.Entities;
using FastTech.Usuarios.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace FastTech.Usuarios.Infra.Persistense;

public static class SeedData
{
    public static async Task EnsureAdminUserAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var email = "admin@admin.com.br";
        var senha = "admin123"; // senha padrão
        var senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);

        if (!context.UserEntities.Any(u => u.Email == email))
        {
            context.UserEntities.Add(new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "Administrador",
                Email = email,
                Cpf = "12345678900",
                PasswordHash = senhaHash,
                Role = UserRole.Admin,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
            Console.WriteLine("Usuário admin criado com sucesso.");
        }
        else
        {
            Console.WriteLine("Usuário admin já existe.");
        }
    }
}