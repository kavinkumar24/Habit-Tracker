
using HabitTracker.Interfaces.Services;

namespace HabitTacker.Services;

public class HabitService : IHabitService
{
    private readonly IHabitRepository _habitRepository;
    public HabitService(IHabitRepository habitRepository)
    {
        _habitRepository = habitRepository;
    }
    public Task<Habit> AddHabitAsync(Habit habit)
    {
        if (habit == null)
        {
            throw new ArgumentNullException(nameof(habit), "Habit cannot be null");
        }

        return _habitRepository.Add(habit);
    }

    public Task DeleteHabitAsync(Guid habitId)
    {
        if (habitId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(habitId), "Habit ID cannot be empty");
        }

        return _habitRepository.Delete(habitId);
    }

    public Task<double> GetCompletionPercentageForDateAsync(Guid userId, DateTime date)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(userId), "User ID cannot be empty");
        }
        if(date == default)
        {
            throw new ArgumentNullException(nameof(date), "Date cannot be default value");
        }

        return _habitRepository.GetCompletionPercentageForDateAsync(userId, date);
    }

    public Task<Habit> GetHabitByIdAsync(Guid habitId)
    {
        if (habitId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(habitId), "Habit ID cannot be empty");
        }

        return _habitRepository.GetByIdAsync(habitId);
    }

    public Task<IEnumerable<Habit>> GetHabitsByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(userId), "User ID cannot be empty");
        }
        
        return _habitRepository.GetHabitsByUserIdAsync(userId);
    }

    public Task<int> GetStreakCountAsync(Guid habitId)
    {
        if (habitId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(habitId), "Habit ID cannot be empty");
        }
        
        return _habitRepository.GetStreakCountAsync(habitId);
    }

    public Task<Habit?> UpdateHabitAsync(Guid habitId, Habit updatedHabit)
    {
        if (habitId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(habitId), "Habit ID cannot be empty");
        }
        if (updatedHabit == null)
        {
            throw new ArgumentNullException(nameof(updatedHabit), "Updated habit cannot be null");
        }

        return _habitRepository.Update(habitId, updatedHabit);
    }
}