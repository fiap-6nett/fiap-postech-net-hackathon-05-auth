namespace FastTech.Usuarios.Contract.GenerateTokens;

public class TokensCommandResult
{
    /// <summary>
    ///     Gets or sets the generated access token.
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    ///     Gets or sets the expiration time of the access token in seconds.
    /// </summary>
    public DateTime? AccessTokenExpiresAt { get; set; }
}