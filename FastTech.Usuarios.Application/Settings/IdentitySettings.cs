namespace FastTech.Usuarios.Application.Settings;

public class IdentitySettings
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; }
    public int AccessRefreshTokenMinutes { get; set; }
}