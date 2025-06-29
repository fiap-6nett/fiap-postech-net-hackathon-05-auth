using System.Text;
using FastTech.Usuarios.Contract.CreateClient;
using FluentValidation.TestHelper;

namespace FastTech.Usuarios.Tests;

public class CreateClientValidatorTest
{
    private readonly CreateClientValidator _validatorCreateClientCommand;

    public CreateClientValidatorTest()
    {
        _validatorCreateClientCommand = new CreateClientValidator();
    }

    /// <summary>
    ///     Deve falhar ao validar o contrato CreateClientCommand quando o campo Name estiver vazio.
    /// </summary>
    [Fact]
    public void CreateClientCommand_Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new CreateClientCommand { Name = "" };
        var result = _validatorCreateClientCommand.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    ///     Deve falhar ao validar o contrato CreateClientCommand quando o campo Name tiver menos de 2 caracteres.
    /// </summary>
    [Fact]
    public void CreateClientCommand_Should_Have_Error_When_Name_Is_Too_Short()
    {
        var model = new CreateClientCommand { Name = "A" };
        var result = _validatorCreateClientCommand.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    ///     Deve falhar ao validar o contrato CreateClientCommand quando o campo Email for inválido.
    /// </summary>
    [Fact]
    public void CreateClientCommand_Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new CreateClientCommand { Email = "invalid-email" };
        var result = _validatorCreateClientCommand.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    /// <summary>
    ///     Deve falhar ao validar o contrato CreateClientCommand quando o campo Cpf estiver vazio.
    /// </summary>
    [Fact]
    public void CreateClientCommand_Should_Have_Error_When_Cpf_Is_Empty()
    {
        var model = new CreateClientCommand { Cpf = "" };
        var result = _validatorCreateClientCommand.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    /// <summary>
    ///     Deve falhar ao validar o contrato CreateClientCommand quando o campo Cpf for inválido (mesmo com máscara).
    /// </summary>
    [Fact]
    public void CreateClientCommand_Should_Have_Error_When_Cpf_Is_Invalid()
    {
        var model = new CreateClientCommand { Cpf = "123.456.789-00" }; // CPF inválido
        var result = _validatorCreateClientCommand.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    /// <summary>
    ///     Deve falhar ao validar o contrato CreateClientCommand quando a senha não estiver codificada em Base64.
    /// </summary>
    [Fact]
    public void CreateClientCommand_Should_Have_Error_When_Password_Is_Not_Base64()
    {
        var model = new CreateClientCommand { PasswordBase64 = "senha_simples" };
        var result = _validatorCreateClientCommand.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PasswordBase64);
    }

    /// <summary>
    ///     Deve passar ao validar o contrato CreateClientCommand quando todos os campos forem válidos.
    /// </summary>
    [Fact]
    public void CreateClientCommand_Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var model = new CreateClientCommand
        {
            Name = "João da Silva",
            Email = "joao@email.com",
            Cpf = "12345678909", // CPF válido
            PasswordBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("senha123"))
        };

        var result = _validatorCreateClientCommand.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}