using FastTech.Usuarios.Domain.Entities;
using FastTech.Usuarios.Domain.Enums;
using FluentValidation;

namespace FastTech.Usuarios.Contract.GenerateTokens;

public class TokensValidator : AbstractValidator<TokensCommand>
{
    public TokensValidator()
    {
        RuleFor(x => x.User)
            .NotEmpty().WithMessage("O campo 'User' é obrigatório.");

        RuleFor(x => x.PasswordBase64)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .Must(UserEntity.IsBase64String).WithMessage("A senha deve estar codificada em Base64.");
    }
}