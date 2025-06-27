using HabitTracker.Services;

namespace HabitTacker.Interfaces.Repositories;

public class HabitCompletionService : IHabitCompletionService
{
    private readonly IHabitCompletionRepository _habitCompletionRepository;

    public HabitCompletionService(IHabitCompletionRepository habitCompletionRepository)
    {
        _habitCompletionRepository = habitCompletionRepository;
    }

    public async Task<bool> MarkAsCompletedAsync(Guid habitId, DateTime date)
    {
        var habit = await _habitCompletionRepository.IsAlreadyCompletedAsync(habitId, date);
        if (habit)
        {
            throw new InvalidOperationException("Habit is already completed for this date.");
        }
        var habitCompletion = new HabitCompletion
        {
            HabitId = habitId,
            DateCompleted = date
        };
        await _habitCompletionRepository.AddAsync(habitCompletion);
        return true;
    }

    public Task<bool> RemoveCompletionAsync(Guid habitId, DateTime date)
    {
        return _habitCompletionRepository.RemoveAsync(habitId, date);
    }
}