using System.Text;
using FastTech.Usuarios.Contract.GenerateTokens;
using FastTech.Usuarios.Domain.Enums;
using FluentValidation.TestHelper;

namespace FastTech.Usuarios.Tests;

public class TokensValidatorTests
{
    private readonly TokensValidator _validator;

    public TokensValidatorTests()
    {
        _validator = new TokensValidator();
    }

    /// <summary>
    ///     Deve falhar quando o tipo de identificador for inválido.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_LoginIdentifierType_Is_Invalid()
    {
        var model = new TokensCommand { LoginIdentifierType = (LoginIdentifierType)999 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.LoginIdentifierType);
    }

    /// <summary>
    ///     Deve falhar quando o campo 'User' estiver vazio.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_User_Is_Empty()
    {
        var model = new TokensCommand
        {
            LoginIdentifierType = LoginIdentifierType.Cpf,
            User = ""
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.User);
    }

    /// <summary>
    ///     Deve falhar quando o tipo for CPF e o valor for inválido.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Cpf_Is_Invalid()
    {
        var model = new TokensCommand
        {
            LoginIdentifierType = LoginIdentifierType.Cpf,
            User = "12345678900" // CPF inválido
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.User);
    }

    /// <summary>
    ///     Deve falhar quando o tipo for Email e o valor for inválido.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new TokensCommand
        {
            LoginIdentifierType = LoginIdentifierType.Email,
            User = "email-invalido"
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.User);
    }

    /// <summary>
    ///     Deve falhar quando a senha estiver vazia.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new TokensCommand
        {
            PasswordBase64 = ""
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PasswordBase64);
    }

    /// <summary>
    ///     Deve falhar quando a senha não estiver em Base64.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Password_Is_Not_Base64()
    {
        var model = new TokensCommand
        {
            PasswordBase64 = "senha123"
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PasswordBase64);
    }

    /// <summary>
    ///     Deve passar quando todos os campos forem válidos (Login via CPF).
    /// </summary>
    [Fact]
    public void Should_Pass_When_All_Fields_Are_Valid_With_Cpf()
    {
        var model = new TokensCommand
        {
            LoginIdentifierType = LoginIdentifierType.Cpf,
            User = "12345678909", // CPF válido
            PasswordBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin123"))
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    ///     Deve passar quando todos os campos forem válidos (Login via Email).
    /// </summary>
    [Fact]
    public void Should_Pass_When_All_Fields_Are_Valid_With_Email()
    {
        var model = new TokensCommand
        {
            LoginIdentifierType = LoginIdentifierType.Email,
            User = "admin@admin.com",
            PasswordBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin123"))
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}