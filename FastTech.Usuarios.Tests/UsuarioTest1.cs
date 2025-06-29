using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastTech.Usuarios.Application.Interfaces;
using FastTech.Usuarios.Application.Services;
using FastTech.Usuarios.Application.Settings;
using FastTech.Usuarios.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;

// ou o caminho real

namespace FastTech.Usuarios.Tests;

public class UsuarioTest1
{
    /// <summary>
    ///     Testa se as configurações da seção "Identity" do appsettings.json são corretamente vinculadas ao objeto IdentitySettings.
    ///     Este teste simula o carregamento de configurações a partir de um dicionário em memória que representa
    ///     o conteúdo do arquivo `appsettings.json`. Em seguida, registra-se a seção de configuração no serviço
    ///     e verifica-se se os valores foram corretamente atribuídos ao objeto `IdentitySettings`.
    ///     Validações realizadas:
    ///     - Verifica se o emissor (`Issuer`) foi corretamente atribuído.
    ///     - Verifica se o público (`Audience`) foi corretamente atribuído.
    ///     - Verifica se o tempo de expiração do token de acesso está correto.
    ///     - Verifica se o tempo de expiração do refresh token está correto.
    ///     Este teste garante que a configuração do sistema de identidade é carregada corretamente
    ///     e está pronta para ser usada nos serviços de autenticação.
    /// </summary>
    [Fact]
    public void IdentitySettings_Should_Be_Bound_Correctly_From_Configuration()
    {
        // Arrange – cria um dicionário simulando o appsettings.json
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Identity:Issuer", "FastTech.Usuarios" },
            { "Identity:Audience", "FastTech.Usuarios.API" },
            { "Identity:SecretKey", "Ikja1zTw42ZG9SymDttA6Dly1kL0uZ7g2dwvdR7ZeLFnKQn5B9" },
            { "Identity:AccessTokenMinutes", "60" },
            { "Identity:AccessRefreshTokenMinutes", "43200" }
        };

        var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

        var services = new ServiceCollection();
        services.Configure<IdentitySettings>(configuration.GetSection("Identity"));

        var provider = services.BuildServiceProvider();
        var identitySettings = provider.GetRequiredService<IOptions<IdentitySettings>>().Value;

        // Assert – verifica se os valores estão corretos
        Assert.Equal("FastTech.Usuarios", identitySettings.Issuer);
        Assert.Equal("FastTech.Usuarios.API", identitySettings.Audience);
        Assert.Equal(60, identitySettings.AccessTokenMinutes);
        Assert.Equal(43200, identitySettings.AccessRefreshTokenMinutes);
    }

    /// <summary>
    ///     Testa se um token de acesso é gerado corretamente ao fornecer credenciais válidas.
    ///     Este teste simula o cenário de um usuário do tipo Admin autenticando-se via CPF.
    ///     A função `GenerateTokenAsync` é chamada com credenciais válidas e, como resultado,
    ///     espera-se que um token JWT válido seja retornado.
    ///     As validações realizadas são:
    ///     - O token gerado não deve ser nulo ou vazio.
    ///     - O token deve conter um ponto ('.'), indicando a estrutura típica de um JWT (Header.Payload.Signature).
    ///     Este teste assegura que a autenticação com CPF está funcionando corretamente e que
    ///     um token válido é retornado ao usuário autenticado.
    /// </summary>
    [Fact]
    public async Task GenerateTokenAsync_AdminCredentials_ShouldReturnAccessToken()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<UserService>>();
        var mockStore = new Mock<IUserCommandStore>();

        var settings = new IdentitySettings
        {
            Issuer = "FastTech.Usuarios",
            Audience = "FastTech.Usuarios.API",
            SecretKey = "Ikja1zTw42ZG9SymDttA6Dly1kL0uZ7g2dwvdR7ZeLFnKQn5B9", // Chave correta
            AccessTokenMinutes = 60,
            AccessRefreshTokenMinutes = 43200
        };

        var options = Options.Create(settings);
        var userService = new UserService(options, mockLogger.Object, mockStore.Object);
        var newId = Guid.NewGuid();

        // Act
        var token = userService.GenerateTokenJwt(newId, UserRole.Admin);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token.AccessToken));
        Assert.Contains(".", token.AccessToken); // simples validação de estrutura JWT
    }

    /// <summary>
    ///     Testa se o token JWT gerado contém os *claims* esperados de um usuário autenticado.
    ///     Este teste realiza a geração de um token para um usuário do tipo Admin e valida os seguintes aspectos:
    ///     - O token não deve ser nulo ou vazio.
    ///     - Deve conter o *claim* de papel (`Role`) com valor igual a "Admin".
    ///     - Deve conter o *claim* de identificação (`sub`) com um valor que seja um GUID válido.
    ///     Esse teste assegura que o processo de geração de tokens está atribuindo corretamente
    ///     as informações necessárias para controle de autenticação e autorização no sistema.
    /// </summary>
    [Fact]
    public async Task GenerateTokenAsync_ShouldContainCorrectClaims()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<UserService>>();
        var mockStore = new Mock<IUserCommandStore>();

        var settings = new IdentitySettings
        {
            Issuer = "FastTech.Usuarios",
            Audience = "FastTech.Usuarios.API",
            SecretKey = "Ikja1zTw42ZG9SymDttA6Dly1kL0uZ7g2dwvdR7ZeLFnKQn5B9", // Chave correta
            AccessTokenMinutes = 60,
            AccessRefreshTokenMinutes = 43200
        };

        var options = Options.Create(settings);
        var userService = new UserService(options, mockLogger.Object, mockStore.Object);
        var newId = Guid.NewGuid();

        // Act
        var token = userService.GenerateTokenJwt(newId, UserRole.Admin);

        // Decode JWT token
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token.AccessToken);

        var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token.AccessToken));
        Assert.NotNull(roleClaim);
        Assert.NotNull(userIdClaim);
        Assert.Equal(UserRole.Admin.ToString(), roleClaim);
        Assert.True(Guid.TryParse(userIdClaim, out _));
    }

    /// <summary>
    ///     Testa a geração de um token JWT válido para um usuário com perfil Admin.
    ///     Este teste garante que o token gerado contenha os *claims* esperados:
    ///     - O emissor (`Issuer`) deve ser igual ao valor configurado ("FastTech.Usuarios").
    ///     - A audiência (`Audience`) deve corresponder ao destino configurado ("FastTech.Usuarios.API").
    ///     - O *claim* de papel (`Role`) deve refletir corretamente o perfil do usuário (Admin).
    ///     - O *claim* de identificação (`sub`) deve conter um GUID válido representando o ID do usuário.
    ///     Esse teste assegura que o processo de autenticação JWT esteja configurado corretamente
    ///     e que os tokens carreguem as informações necessárias para controle de acesso.
    /// </summary>
    [Fact]
    public async Task GenerateTokenAsync_ValidAdminToken_ShouldContainExpectedClaims()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<UserService>>();
        var mockStore = new Mock<IUserCommandStore>();
        var settings = new IdentitySettings
        {
            Issuer = "FastTech.Usuarios",
            Audience = "FastTech.Usuarios.API",
            SecretKey = "Ikja1zTw42ZG9SymDttA6Dly1kL0uZ7g2dwvdR7ZeLFnKQn5B9", // Chave correta
            AccessTokenMinutes = 60,
            AccessRefreshTokenMinutes = 43200
        };

        var options = Options.Create(settings);
        var userService = new UserService(options, mockLogger.Object, mockStore.Object);
        var newId = Guid.NewGuid();

        // Act
        var token = userService.GenerateTokenJwt(newId, UserRole.Admin);
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token.AccessToken);

        // Assert
        Assert.NotNull(jwt);
        Assert.Equal("FastTech.Usuarios", jwt.Issuer);
        Assert.Equal("FastTech.Usuarios.API", jwt.Audiences.First());

        var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        Assert.Equal(UserRole.Admin.ToString(), roleClaim);
        Assert.True(Guid.TryParse(idClaim, out _));
    }

    /// <summary>
    ///     Testa a validação de token com uma chave secreta incorreta.
    ///     Este teste verifica se o token JWT, gerado com uma chave secreta válida,
    ///     falha ao ser validado com uma chave diferente (incorreta).
    ///     O resultado esperado é que a validação dispare uma exceção de assinatura inválida,
    ///     garantindo assim a integridade e a segurança do token.
    /// </summary>
    [Fact]
    public async Task ValidateTokenAsync_WithWrongSecretKey_ShouldFailValidation()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<UserService>>();
        var mockStore = new Mock<IUserCommandStore>();

        // Configuração usada para GERAR o token
        var settings = new IdentitySettings
        {
            Issuer = "FastTech.Usuarios",
            Audience = "FastTech.Usuarios.API",
            SecretKey = "Ikja1zTw42ZG9SymDttA6Dly1kL0uZ7g2dwvdR7ZeLFnKQn5B9", // Chave correta
            AccessTokenMinutes = 60,
            AccessRefreshTokenMinutes = 43200
        };

        var options = Options.Create(settings);
        var userService = new UserService(options, mockLogger.Object, mockStore.Object);

        var newId = Guid.NewGuid();

        // Gera token com chave correta
        var token = userService.GenerateTokenJwt(newId, UserRole.Admin);

        // Configuração usada para VALIDAR (com chave incorreta)
        var invalidKey = "hZLMzfmJvm2YUF0VeCMDZ3n6sSU9LS1lV5L"; // Chave incorreta

        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(invalidKey)),
            ValidateIssuer = true,
            ValidIssuer = settings.Issuer,
            ValidateAudience = true,
            ValidAudience = settings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        // Assert – deve lançar SecurityTokenInvalidSignatureException
        Assert.ThrowsAny<SecurityTokenInvalidSignatureException>(() => { handler.ValidateToken(token.AccessToken, validationParameters, out _); });
    }


    [Fact]
    public async Task ShouldGenerateAndValidateTokenSuccessfully()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<UserService>>();
        var mockStore = new Mock<IUserCommandStore>();

        var correctKey = "Ikja1zTw42ZG9SymDttA6Dly1kL0uZ7g2dwvdR7ZeLFnKQn5B9";
        var issuer = "FastTech.Usuarios";
        var audience = "FastTech.Usuarios.API";

        var settings = new IdentitySettings
        {
            Issuer = issuer,
            Audience = audience,
            SecretKey = correctKey,
            AccessTokenMinutes = 60,
            AccessRefreshTokenMinutes = 43200
        };

        var options = Options.Create(settings);
        var userService = new UserService(options, mockLogger.Object, mockStore.Object);

        var userId = Guid.NewGuid();
        var token = userService.GenerateTokenJwt(userId, UserRole.Admin);

        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(5),
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(correctKey)),
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.NameIdentifier
        };

        // Act
        var principal = handler.ValidateToken(token.AccessToken, validationParameters, out var validatedToken);

        // Assert
        Assert.NotNull(principal);
        // Assert
        Assert.NotNull(principal);
        Assert.Equal(userId.ToString(), principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Assert.Equal("Admin", principal.FindFirst(ClaimTypes.Role)?.Value);
    }

    [Fact]
    public void HashPassword_And_Verify_ShouldSucceed()
    {
        // Este teste verifica se uma senha pode ser criptografada (hash) corretamente
        // e, em seguida, validada com sucesso usando a mesma senha original.

        // Arrange – define a senha original em texto puro
        var plainPassword = "admin123";

        // Act – gera um hash da senha usando BCrypt
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

        // Verifica se a senha original corresponde ao hash gerado
        var isMatch = BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);

        // Assert – o hash não deve estar vazio e a verificação deve retornar verdadeiro
        Assert.False(string.IsNullOrWhiteSpace(hashedPassword), "O hash gerado não deve ser nulo ou vazio.");
        Assert.True(isMatch, "A verificação da senha deve ter sucesso com a senha original.");
    }

    [Fact]
    public void HashPassword_WithWrongPassword_ShouldFailVerification()
    {
        // Este teste garante que uma senha incorreta não passe na validação
        // de um hash gerado a partir de uma senha diferente.

        // Arrange – define a senha correta e uma senha incorreta
        var correctPassword = "admin123";
        var wrongPassword = "admin456";

        // Gera o hash da senha correta
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(correctPassword);

        // Act – tenta verificar o hash usando a senha incorreta
        var isMatch = BCrypt.Net.BCrypt.Verify(wrongPassword, hashedPassword);

        // Assert – a verificação deve falhar, pois a senha fornecida está errada
        Assert.False(isMatch, "A verificação deve falhar quando a senha está incorreta.");
    }
}