
using HabitTacker.Contexts;
using HabitTacker.Interfaces.Repositories;
using HabitTacker.Models;
using HabitTacker.Repositories;
using Microsoft.EntityFrameworkCore;
namespace HabitTacker.Repositories;
public class UserRepository : Repository<Guid, User>, IUserRepository
{

    public UserRepository(HabitTrackerContext habitTrackerContext) : base(habitTrackerContext)
    {}
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await _habitTrackerContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
       
        var user = await _habitTrackerContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user;
    }

    public async Task<User?> GetWithHabitsAsync(Guid userId)
    {
        var user = await _habitTrackerContext.Users
            .Include(u => u.Habits)
            .ThenInclude(h => h.Completions)
            .FirstOrDefaultAsync(u => u.Id == userId);
        return user;
    }
}