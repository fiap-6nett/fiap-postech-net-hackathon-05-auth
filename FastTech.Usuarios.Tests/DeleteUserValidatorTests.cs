using FastTech.Usuarios.Contract.DeleteUser;
using FluentValidation.TestHelper;

namespace FastTech.Usuarios.Tests;

public class DeleteUserValidatorTests
{
    private readonly DeleteUserValidator _validator;

    public DeleteUserValidatorTests()
    {
        _validator = new DeleteUserValidator();
    }

    /// <summary>
    ///     Deve falhar quando o ID estiver vazio (Guid.Empty).
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var model = new DeleteUserCommand { Id = Guid.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    /// <summary>
    ///     Deve passar quando o ID for um GUID válido.
    /// </summary>
    [Fact]
    public void Should_Pass_When_Id_Is_Valid()
    {
        var model = new DeleteUserCommand { Id = Guid.NewGuid() };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}