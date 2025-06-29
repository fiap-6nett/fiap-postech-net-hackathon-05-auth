using FastTech.Usuarios.Contract.GetUserById;
using FluentValidation.TestHelper;

namespace FastTech.Usuarios.Tests;

public class GetUserByIdValidatorTests
{
    private readonly GetUserByIdValidator _validator;

    public GetUserByIdValidatorTests()
    {
        _validator = new GetUserByIdValidator();
    }

    /// <summary>
    ///     Deve falhar quando o ID estiver vazio (Guid.Empty).
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var model = new GetUserByIdQuery { TargetUserId = Guid.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.TargetUserId);
    }

    /// <summary>
    ///     Deve passar quando o ID for um GUID válido.
    /// </summary>
    [Fact]
    public void Should_Pass_When_Id_Is_Valid()
    {
        var model = new GetUserByIdQuery { TargetUserId = Guid.NewGuid() };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.TargetUserId);
    }
}