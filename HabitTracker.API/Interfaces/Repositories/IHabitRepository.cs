using HabitTacker.Interfaces;

public interface IHabitRepository : IRepository<Guid, Habit>
{
    Task<IEnumerable<Habit>> GetHabitsByUserIdAsync(Guid userId);
    Task<int> GetTotalHabitsCountAsync(Guid userId);
    Task<double> GetCompletionPercentageForDateAsync(Guid userId, DateTime date);
    Task<int> GetStreakCountAsync(Guid habitId);
    Task<Habit> GetByIdAsync(Guid habitId);
}
