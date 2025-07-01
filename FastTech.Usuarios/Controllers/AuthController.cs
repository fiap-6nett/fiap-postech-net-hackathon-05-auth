using FastTech.Usuarios.Application.Interfaces;
using FastTech.Usuarios.Contract.GenerateTokens;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FastTech.Usuarios.Controllers;

[Route("api/v1.0/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IUserService _userService;
    private readonly IValidator<TokensCommand> _validatorTokensCommand;


    public AuthController(ILogger<AuthController> logger, IUserService userService, IValidator<TokensCommand> validatorTokensCommand)
    {
        _logger = logger;
        _userService = userService;
        _validatorTokensCommand = validatorTokensCommand;
    }


    /// <summary>
    ///     Gera tokens de autenticação (access token e refresh token) com base nas credenciais fornecidas.
    /// </summary>
    /// <param name="payload">
    ///     Objeto contendo o nome de usuário e a senha codificada em Base64.
    ///     <para>Exemplo:</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <description>Email: <c>admin@admin.com.br</c></description>
    ///             <description>CPF: <c>82909117006</c></description>
    ///         </item>
    ///         <item>
    ///             <description>PasswordBase64: <c>YWRtaW4xMjM=</c> (Base64 de <c>admin123</c>)</description>
    ///         </item>
    ///     </list>
    /// </param>
    /// <returns>
    ///     Retorna um <see cref="TokensCommandResult" /> com os tokens de acesso e atualização,
    ///     além dos tempos de expiração de cada um.
    /// </returns>
    /// <response code="200">Token gerado com sucesso.</response>
    /// <response code="400">Requisição inválida. Dados de entrada ausentes ou malformados.</response>
    /// <response code="500">Erro interno ao processar a requisição.</response>
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokensCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetToken([FromBody] TokensCommand payload)
    {
        try
        {
            await _validatorTokensCommand.ValidateAndThrowAsync(payload);
            var token = await _userService.GenerateTokenAsync(payload.User, payload.PasswordBase64);
            return new OkObjectResult(new TokensCommandResult
            {
                AccessToken = token.AccessToken,
                AccessTokenExpiresAt = token.ExpiresAt
            });
        }
        catch (Exception ex)
        {
            var message = "Erro ao gerar token: {ex.Message}";
            _logger.LogError(message);
            return new BadRequestObjectResult(new
            {
                error = message,
                details = ex.Message
            });
        }
    }
}