using HabitTacker.Models;

namespace HabitTacker.Interfaces.Repositories;

public interface IUserRepository : IRepository<Guid, User>
{

    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetWithHabitsAsync(Guid userId);
}