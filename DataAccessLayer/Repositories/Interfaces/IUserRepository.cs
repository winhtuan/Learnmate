using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(long id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
}
