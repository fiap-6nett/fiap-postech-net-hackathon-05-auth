using FastTech.Usuarios.Domain.Entities;
using FastTech.Usuarios.Domain.Enums;

namespace FastTech.Usuarios.Application.Interfaces;

public interface IUserService
{
    Task<TokenEntity> GenerateTokenAsync(string user, string passwordBase64);
    Task<UserEntity> RegisterUserAsync(string name, string cpf, string email, string passwordBase64, UserRole role);
    TokenEntity GenerateTokenJwt(Guid id, UserRole role);
    Task<UserEntity?> GetUserByIdAsync(Guid targetUserId, string requestingUserId, string requestingUserRole);
    Task<UserEntity?> DeleteUserAsync(Guid targetUserId, string requestingUserId, string requestingUserRole);
    Task<UserEntity> UpdateUserAsync(Guid targetUserId, string name, string cpf, string email, string passwordBase64, string requestingUserId, string requestingUserRole);
}