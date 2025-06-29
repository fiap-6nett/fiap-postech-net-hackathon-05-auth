namespace FastTech.Usuarios.Domain.Entities;

public class TokenEntity
{
    public string AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}