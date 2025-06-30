using FastTech.Usuarios.Domain.Entities;
using FastTech.Usuarios.Domain.Enums;
using FluentValidation;

namespace FastTech.Usuarios.Contract.CreateEmployee;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MinimumLength(2).WithMessage("O nome deve ter no mínimo 2 caracteres.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("O e-mail informado é inválido.");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Must(UserEntity.IsValidCpf).WithMessage("O CPF informado é inválido.");

        RuleFor(x => x.PasswordBase64)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .Must(UserEntity.IsBase64String).WithMessage("A senha deve estar codificada em Base64.");

        RuleFor(x => x.Role)
            .IsInEnum()
            .Must(role => role == UserRole.Admin || role == UserRole.Manager || role == UserRole.Employee || role == UserRole.KitchenStaff)
            .WithMessage("O perfil do funcionário deve ser Admin, Manager ou Employee.");
    }
}