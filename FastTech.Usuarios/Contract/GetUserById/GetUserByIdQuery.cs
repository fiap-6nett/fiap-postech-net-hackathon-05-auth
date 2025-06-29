namespace FastTech.Usuarios.Contract.GetUserById;

public class GetUserByIdQuery
{
    /// <summary>
    ///     Identificador único do usuário que será consultado no sistema.
    ///     <para>Exemplo: <c>d290f1ee-6c54-4b01-90e6-d701748f0851</c></para>
    /// </summary>
    public Guid TargetUserId { get; set; }

    /// <summary>
    ///     Identificador do usuário autenticado que está realizando a consulta.
    ///     Esse valor é extraído do token JWT.
    /// </summary>
    public string? RequestingUserId { get; set; }

    /// <summary>
    ///     Papel (role) do usuário autenticado que está realizando a consulta.
    ///     Pode ser usado para aplicar regras de autorização e acesso condicional.
    /// </summary>
    public string RequestingUserRole { get; set; }
}