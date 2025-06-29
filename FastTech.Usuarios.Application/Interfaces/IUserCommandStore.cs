using FastTech.Usuarios.Domain.Entities;

namespace FastTech.Usuarios.Application.Interfaces;

public interface IUserCommandStore
{
    /// <summary>
    ///     Cadastra um novo usuário no sistema.
    /// </summary>
    /// <param name="user">Entidade do usuário a ser criada.</param>
    Task CreateUserAsync(UserEntity user);

    /// <summary>
    ///     Atualiza os dados de um usuário existente.
    /// </summary>
    /// <param name="user">Entidade do usuário com os dados atualizados.</param>
    Task UpdateUserAsync(UserEntity user);

    /// <summary>
    ///     Exclui um usuário com base no identificador informado.
    /// </summary>
    /// <param name="id">Identificador único do usuário.</param>
    Task DeleteUserAsync(Guid id);

    /// <summary>
    ///     Consulta um usuário pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único do usuário.</param>
    /// <returns>Entidade do usuário, se encontrada.</returns>
    Task<UserEntity?> GetUserByIdAsync(Guid id);

    /// <summary>
    ///     Consulta um usuário pelo CPF e pela senha já codificada.
    /// </summary>
    /// <param name="cpf">CPF do usuário.</param>
    /// <returns>Entidade do usuário, se encontrada.</returns>
    Task<UserEntity?> GetUserByCpfAndPasswordAsync(string cpf);

    /// <summary>
    ///     Consulta um usuário pelo e-mail e pela senha já codificada.
    /// </summary>
    /// <param name="email">E-mail do usuário.</param>
    /// <returns>Entidade do usuário, se encontrada.</returns>
    Task<UserEntity?> GetUserByEmailAndPasswordAsync(string emai);

    /// <summary>
    ///     Verifica se já existe um usuário com o e-mail ou CPF informado.
    /// </summary>
    /// <param name="email">E-mail do usuário a verificar.</param>
    /// <param name="cpf">CPF do usuário a verificar.</param>
    /// <returns><c>true</c> se já existir um usuário com o mesmo e-mail ou CPF; caso contrário, <c>false</c>.</returns>
    Task<bool> ExistsByEmailOrCpfAsync(string email, string cpf);
}