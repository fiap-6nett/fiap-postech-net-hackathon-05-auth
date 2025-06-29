using FastTech.Usuarios.Domain.Enums;

namespace FastTech.Usuarios.Contract.CreateEmployee;

public class CreateEmployeeCommandResult
{
    /// <summary>
    ///     Identificador único do cliente no sistema.
    ///     <para>Exemplo: <c>d290f1ee-6c54-4b01-90e6-d701748f0851</c></para>
    /// </summary>
    public Guid Id { get; set; }

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
    ///     <para>Exemplo: <c>12345678900</c></para>
    /// </summary>
    public string Cpf { get; set; }

    /// <summary>
    ///     Role of the user in the system.
    /// </summary>
    public UserRole Role { get; set; }
}