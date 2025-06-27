namespace HabitTacker.Interfaces.Repositories;

public interface IHabitCompletionRepository
{
    Task<HabitCompletion> AddAsync(HabitCompletion habitCompletion);
    Task<bool> RemoveAsync(Guid habitCompletionId, DateTime date);
    Task<bool> IsAlreadyCompletedAsync(Guid habitId, DateTime date);

}