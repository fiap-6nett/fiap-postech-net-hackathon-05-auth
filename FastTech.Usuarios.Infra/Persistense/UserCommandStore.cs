using FastTech.Usuarios.Application.Interfaces;
using FastTech.Usuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastTech.Usuarios.Infra.Persistense;

public class UserCommandStore : IUserCommandStore
{
    private readonly AppDbContext _context;

    public UserCommandStore(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateUserAsync(UserEntity user)
    {
        await _context.UserEntities.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public Task UpdateUserAsync(UserEntity user)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _context.UserEntities.FindAsync(id);
        if (user == null) return;

        user.IsAvailable = false;
        user.LastUpdatedAt = DateTime.UtcNow;

        _context.UserEntities.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<UserEntity?> GetUserByIdAsync(Guid id)
    {
        return await _context.UserEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<UserEntity?> GetUserByCpfAndPasswordAsync(string cpf)
    {
        return await _context.UserEntities.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Cpf == cpf && u.IsAvailable);
    }

    public async Task<UserEntity?> GetUserByEmailAndPasswordAsync(string email)
    {
        return await _context.UserEntities.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email && u.IsAvailable);
    }

    public async Task<bool> ExistsByEmailOrCpfAsync(string email, string cpf)
    {
        return await _context.UserEntities
            .AsNoTracking()
            .AnyAsync(u => u.IsAvailable && (u.Email == email || u.Cpf == cpf));
    }
}