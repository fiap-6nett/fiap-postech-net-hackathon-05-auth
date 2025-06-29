using System.Text;
using FastTech.Usuarios.Domain.Enums;

namespace FastTech.Usuarios.Domain.Entities;

/// <summary>
///     Data transfer object representing a registered user.
/// </summary>
public class UserEntity : BaseEntity
{
    /// <summary>
    ///     Unique identifier for the user.
    ///     <para>Example: <c>d290f1ee-6c54-4b01-90e6-d701748f0851</c></para>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Full name of the user.
    ///     <para>Example: <c>John Doe</c></para>
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Hashed password of the user (stored securely).
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    ///     CPF of the user (Brazilian national identification number).
    ///     <para>Example: <c>12345678900</c></para>
    /// </summary>
    public string Cpf { get; set; }

    /// <summary>
    ///     Email address of the user.
    ///     <para>Example: <c>user@example.com</c></para>
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     Role of the user in the system (e.g., Admin, Manager, Client).
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    ///     Timestamp when the user account was created (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Timestamp of the last update to the user account (UTC).
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }

    /// <summary>
    ///     Validates the CPF (Cadastro de Pessoas Físicas) number.
    /// </summary>
    /// <param name="cpf"></param>
    /// <returns></returns>
    public static bool IsValidCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        // Remove caracteres especiais
        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
            return false;

        // Elimina CPFs com todos os dígitos iguais (ex: 00000000000, 11111111111)
        if (cpf.Distinct().Count() == 1)
            return false;

        // Validação do primeiro dígito verificador
        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += (cpf[i] - '0') * (10 - i);

        var firstDigit = sum % 11 < 2 ? 0 : 11 - sum % 11;
        if (cpf[9] - '0' != firstDigit)
            return false;

        // Validação do segundo dígito verificador
        sum = 0;
        for (var i = 0; i < 10; i++)
            sum += (cpf[i] - '0') * (11 - i);

        var secondDigit = sum % 11 < 2 ? 0 : 11 - sum % 11;
        if (cpf[10] - '0' != secondDigit)
            return false;

        return true;
    }

    /// <summary>
    ///     Valida se a string fornecida está corretamente codificada em Base64.
    /// </summary>
    /// <param name="value">Texto a ser validado.</param>
    /// <returns><c>true</c> se for uma string Base64 válida; caso contrário, <c>false</c>.</returns>
    public static bool IsBase64String(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        try
        {
            var buffer = Convert.FromBase64String(value);
            var result = Encoding.UTF8.GetString(buffer);

            // Opcional: valida se a re-codificação gera a mesma string original
            var encodedAgain = Convert.ToBase64String(Encoding.UTF8.GetBytes(result));
            return value.TrimEnd('=') == encodedAgain.TrimEnd('=');
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     Remove todos os caracteres que não são dígitos de uma string.
    /// </summary>
    /// <param name="value">Texto de entrada, como CPF, CNPJ ou número de telefone.</param>
    /// <returns>Somente os dígitos numéricos.</returns>
    public static string SomenteNumeros(string value)
    {
        return new string(value?.Where(char.IsDigit).ToArray());
    }
}