namespace HabitTracker.Interfaces.Services;

public interface IHabitService
{
    Task<Habit> AddHabitAsync(Habit habit);
    Task<IEnumerable<Habit>> GetHabitsByUserIdAsync(Guid userId);
    Task<Habit> GetHabitByIdAsync(Guid habitId);
    Task<Habit?> UpdateHabitAsync(Guid habitId, Habit updatedHabit);
    Task DeleteHabitAsync(Guid habitId);
    Task<double> GetCompletionPercentageForDateAsync(Guid userId, DateTime date);
    Task<int> GetStreakCountAsync(Guid habitId);
}