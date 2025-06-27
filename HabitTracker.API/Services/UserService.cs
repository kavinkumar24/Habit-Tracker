

using HabitTacker.Interfaces.Repositories;
using HabitTacker.Models;
using HabitTracker.Interfaces.Services;

namespace HabitTacker.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Task<User> AddUserAsync(User user)
    {
        
        return _userRepository.Add(user);
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email), "Email cannot be null or empty");
        }
        return _userRepository.GetUserByEmailAsync(email);
    }

    public Task<User?> GetUserByIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(userId), "User ID cannot be empty");
        }
        return _userRepository.Get(userId);
    }

    public Task<User?> GetUserByUsernameAsync(string username)
    {
        if(username == null)
        {
            throw new ArgumentNullException(nameof(username), "Username cannot be null");
        }
        return _userRepository.GetUserByUsernameAsync(username);
    }

    public async Task<User> GetUserWithHabitsAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(userId), "User ID cannot be empty");
        }

        var user = await _userRepository.GetWithHabitsAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found with habits.");
        }
        return user;
    }
}