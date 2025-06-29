namespace FastTech.Usuarios.Domain.Entities;

public abstract class BaseEntity
{
    public bool IsAvailable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastUpdatedAt { get; set; }
}