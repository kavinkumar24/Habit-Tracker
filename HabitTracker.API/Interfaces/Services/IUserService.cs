using HabitTacker.Models;

namespace HabitTracker.Interfaces.Services;

public interface IUserService
{
    Task<User> AddUserAsync(User user);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User> GetUserWithHabitsAsync(Guid userId);
}
