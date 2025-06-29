using System.Text.Json.Serialization;

namespace FastTech.Usuarios.Contract.UpdateUser;

public class UpdateUserCommand
{
    /// <summary>
    ///     Identificador único do cliente no sistema.
    ///     <para>Exemplo: <c>d290f1ee-6c54-4b01-90e6-d701748f0851</c></para>
    /// </summary>
    [JsonIgnore]
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
    ///     Senha do cliente codificada em Base64.
    ///     <para>Exemplo (Base64 de <c>admin123</c>): <c>YWRtaW4xMjM=</c></para>
    /// </summary>
    public string PasswordBase64 { get; set; }
}