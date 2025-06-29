using FastTech.Usuarios.Domain.Entities;
using FastTech.Usuarios.Domain.Enums;
using FluentValidation;

namespace FastTech.Usuarios.Contract.GenerateTokens;

public class TokensValidator : AbstractValidator<TokensCommand>
{
    public TokensValidator()
    {
        RuleFor(x => x.LoginIdentifierType)
            .IsInEnum()
            .WithMessage("O tipo de identificador deve ser CPF ou Email.");

        RuleFor(x => x.User)
            .NotEmpty().WithMessage("O campo 'User' é obrigatório.")
            .DependentRules(() =>
            {
                When(x => x.LoginIdentifierType == LoginIdentifierType.Cpf, () =>
                {
                    RuleFor(x => x.User)
                        .Must(UserEntity.IsValidCpf).WithMessage("O CPF informado é inválido.");
                });

                When(x => x.LoginIdentifierType == LoginIdentifierType.Email, () =>
                {
                    RuleFor(x => x.User)
                        .EmailAddress().WithMessage("O e-mail informado é inválido.");
                });
            });

        RuleFor(x => x.PasswordBase64)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .Must(UserEntity.IsBase64String).WithMessage("A senha deve estar codificada em Base64.");
    }
}