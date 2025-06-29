using FastTech.Usuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastTech.Usuarios.Infra.Persistense;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> UserEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // TODO Adicione outras configurações de mapeamento, se necessário.
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("UserEntities");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Cpf).IsRequired();
            entity.Property(e => e.Role).IsRequired();
            entity.Property(e => e.IsAvailable).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd();
            entity.Property(e => e.LastUpdatedAt);
        });
    }
}