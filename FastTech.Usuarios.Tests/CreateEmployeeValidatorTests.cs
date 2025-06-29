using System.Text;
using FastTech.Usuarios.Contract.CreateEmployee;
using FastTech.Usuarios.Domain.Enums;
using FluentValidation.TestHelper;

namespace FastTech.Usuarios.Tests;

public class CreateEmployeeValidatorTests
{
    private readonly CreateEmployeeValidator _validator;

    public CreateEmployeeValidatorTests()
    {
        _validator = new CreateEmployeeValidator();
    }

    /// <summary>
    ///     Deve falhar quando o nome estiver vazio.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new CreateEmployeeCommand { Name = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    ///     Deve falhar quando o nome for muito curto.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Short()
    {
        var model = new CreateEmployeeCommand { Name = "A" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    ///     Deve falhar quando o nome for muito longo.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Long()
    {
        var model = new CreateEmployeeCommand { Name = new string('A', 101) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    ///     Deve falhar quando o e-mail estiver vazio.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new CreateEmployeeCommand { Email = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    /// <summary>
    ///     Deve falhar quando o e-mail for inválido.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new CreateEmployeeCommand { Email = "email-invalido" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    /// <summary>
    ///     Deve falhar quando o CPF estiver vazio.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Cpf_Is_Empty()
    {
        var model = new CreateEmployeeCommand { Cpf = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    /// <summary>
    ///     Deve falhar quando o CPF for inválido.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Cpf_Is_Invalid()
    {
        var model = new CreateEmployeeCommand { Cpf = "123.456.789-00" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    /// <summary>
    ///     Deve falhar quando a senha estiver vazia.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new CreateEmployeeCommand { PasswordBase64 = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PasswordBase64);
    }

    /// <summary>
    ///     Deve falhar quando a senha não estiver em Base64.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Password_Is_Not_Base64()
    {
        var model = new CreateEmployeeCommand
        {
            Name = "Teste",
            Email = "teste@email.com",
            Cpf = "12345678909",
            PasswordBase64 = "senha123", // ❌ inválido
            Role = UserRole.Employee
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PasswordBase64);
    }


    /// <summary>
    ///     Deve falhar quando o perfil não for Admin, Manager ou Employee.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Role_Is_Invalid()
    {
        var model = new CreateEmployeeCommand { Role = UserRole.Customer };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    /// <summary>
    ///     Deve passar quando todos os campos forem válidos.
    /// </summary>
    [Fact]
    public void Should_Pass_When_All_Fields_Are_Valid()
    {
        var model = new CreateEmployeeCommand
        {
            Name = "Maria Gerente",
            Email = "maria@empresa.com",
            Cpf = "12345678909", // CPF válido
            PasswordBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("senhaSegura")),
            Role = UserRole.Manager
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}