using FluentValidation;

namespace FastTech.Usuarios.Contract.DeleteUser;

public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.Id)
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