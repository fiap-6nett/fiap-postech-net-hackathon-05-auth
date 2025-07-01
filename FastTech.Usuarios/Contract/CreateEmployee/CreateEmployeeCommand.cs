using FastTech.Usuarios.Domain.Enums;

namespace FastTech.Usuarios.Contract.CreateEmployee;

public class CreateEmployeeCommand
{
    /// <summary>
    ///     Nome completo do cliente.
    ///     <para>Exemplo: <c>João da Silva</c></para>
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Endereço de e-mail do cliente.
    ///     <para>Exemplo: <c>joao.silva@email.com</c></para>
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     CPF do cliente, contendo apenas números.
    ///     <para>Exemplo: <c>82909117006</c></para>
    /// </summary>
    public string Cpf { get; set; }

    /// <summary>
    ///     Senha do cliente codificada em Base64.
    ///     <para>Exemplo (Base64 de <c>admin123</c>): <c>YWRtaW4xMjM=</c></para>
    /// </summary>
    public string PasswordBase64 { get; set; }

    /// <summary>
    ///     Role of the user in the system.
    /// </summary>
    public UserRole Role { get; set; }
}