
using HabitTacker.Contexts;
using HabitTacker.Interfaces;
using HabitTacker.Models;
using HabitTacker.Repositories;
using Microsoft.EntityFrameworkCore;

public class UserRepository : Repository<Guid, User>, IUserRepository
{

    public UserRepository(HabitTrackerContext habitTrackerContext) : base(habitTrackerContext)
    {}
    public async Task<User> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException("Email cannot be null or empty.");
        }
        var user = await _habitTrackerContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new InvalidOperationException($"User with email '{email}' not found.");
        }
        return user;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentNullException("Username cannot be null or empty.");
        }
        var user = await _habitTrackerContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            throw new InvalidOperationException($"User with username '{username}' not found.");
        }
        
        return user;
    }

    public async Task<User> GetWithHabitsAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException("User ID cannot be empty.");
        }
        
        var user = await _habitTrackerContext.Users
            .Include(u => (IEnumerable<Habit>)u.Habits!)
            .ThenInclude(h => h.Completions)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new InvalidOperationException($"User with ID '{userId}' not found.");
        }

        return user;
    }
}