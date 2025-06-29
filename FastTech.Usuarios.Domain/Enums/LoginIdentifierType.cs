using System.ComponentModel.DataAnnotations;

namespace FastTech.Usuarios.Domain.Enums;

/// <summary>
///     Define os tipos de identificadores aceitos para autenticação do usuário.
/// </summary>
public enum LoginIdentifierType
{
    /// <summary>
    ///     Autenticação utilizando o CPF do usuário.
    /// </summary>
    [Display(Name = "Cpf")] Cpf = 1,

    /// <summary>
    ///     Autenticação utilizando o e-mail do usuário.
    /// </summary>
    [Display(Name = "Email")] Email = 2
}