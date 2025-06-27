
using HabitTacker.Contexts;
using HabitTacker.Interfaces.Repositories;

public class HabitCompletionRepository : IHabitCompletionRepository
{
    private readonly HabitTrackerContext _habitTrackerContext;
    public HabitCompletionRepository(HabitTrackerContext habitTrackerContext)
    {
        _habitTrackerContext = habitTrackerContext;
    }

    public Task<HabitCompletion> AddAsync(HabitCompletion habitCompletion)
    {
        _habitTrackerContext.HabitCompletions.Add(habitCompletion);
        _habitTrackerContext.SaveChanges();
        return Task.FromResult(habitCompletion);
    }

    public Task<bool> IsAlreadyCompletedAsync(Guid habitId, DateTime date)
    {
        var exists = _habitTrackerContext.HabitCompletions
            .Any(hc => hc.HabitId == habitId && hc.DateCompleted.Date == date.Date);
        return Task.FromResult(exists);
    }

    public Task<bool> RemoveAsync(Guid habitCompletionId, DateTime date)
    {
        var habitCompletion = _habitTrackerContext.HabitCompletions
            .FirstOrDefault(hc => hc.Id == habitCompletionId && hc.DateCompleted.Date == date.Date);
        
        if (habitCompletion != null)
        {
            _habitTrackerContext.HabitCompletions.Remove(habitCompletion);
            _habitTrackerContext.SaveChanges();
            return Task.FromResult(true);
        }
        
        return Task.FromResult(false);
    }
}