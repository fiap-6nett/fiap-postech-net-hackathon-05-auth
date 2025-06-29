using System.Text;
using FastTech.Usuarios.Contract.UpdateUser;
using FluentValidation.TestHelper;

namespace FastTech.Usuarios.Tests;

public class UpdateUserValidatorTests
{
    private readonly UpdateUserValidator _validator;

    public UpdateUserValidatorTests()
    {
        _validator = new UpdateUserValidator();
    }

    /// <summary>
    ///     Deve falhar quando o ID estiver vazio.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var model = new UpdateUserCommand { Id = Guid.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    /// <summary>
    ///     Deve falhar quando o nome estiver vazio.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new UpdateUserCommand { Name = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    ///     Deve falhar quando o nome for muito curto.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Short()
    {
        var model = new UpdateUserCommand { Name = "A" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    ///     Deve falhar quando o nome for muito longo.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Long()
    {
        var model = new UpdateUserCommand { Name = new string('A', 101) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    ///     Deve falhar quando o e-mail estiver vazio.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new UpdateUserCommand { Email = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    /// <summary>
    ///     Deve falhar quando o e-mail for inválido.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new UpdateUserCommand { Email = "email-invalido" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    /// <summary>
    ///     Deve falhar quando o CPF estiver vazio.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Cpf_Is_Empty()
    {
        var model = new UpdateUserCommand { Cpf = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    /// <summary>
    ///     Deve falhar quando o CPF for inválido.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Cpf_Is_Invalid()
    {
        var model = new UpdateUserCommand { Cpf = "123.456.789-00" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    /// <summary>
    ///     Deve falhar quando a senha estiver vazia.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new UpdateUserCommand { PasswordBase64 = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PasswordBase64);
    }

    /// <summary>
    ///     Deve falhar quando a senha não estiver em Base64.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Password_Is_Not_Base64()
    {
        var model = new UpdateUserCommand { PasswordBase64 = "senha123" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PasswordBase64);
    }

    /// <summary>
    ///     Deve passar quando todos os campos forem válidos.
    /// </summary>
    [Fact]
    public void Should_Pass_When_All_Fields_Are_Valid()
    {
        var model = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Name = "João da Silva",
            Email = "joao@email.com",
            Cpf = "12345678909", // CPF válido
            PasswordBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("senha123"))
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}