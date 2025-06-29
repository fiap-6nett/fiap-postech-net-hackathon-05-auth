using FluentValidation;

namespace FastTech.Usuarios.Contract.GetUserById;

public class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(x => x.TargetUserId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório.")
            .Must(BeAValidGuid).WithMessage("O ID fornecido não é um GUID válido.");
    }

    /// <summary>
    ///     Verifica se o Guid é válido (diferente de Guid.Empty).
    /// </summary>
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}