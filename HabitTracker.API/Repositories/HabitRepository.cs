using HabitTacker.Repositories;
using HabitTacker.Contexts;
using HabitTacker.Models;
using Microsoft.EntityFrameworkCore;
namespace HabitTacker.Repositories;

public class HabitRepository : Repository<Guid, Habit>, IHabitRepository
{
    public HabitRepository(HabitTrackerContext context) : base(context) { }

    public async Task<IEnumerable<Habit>> GetHabitsByUserIdAsync(Guid userId)
    {
        var habits = await _dbSet.Where(h => h.UserId == userId).ToListAsync();
        if (!habits.Any())
        {
            throw new NotFoundException($"No habits found for user with ID '{userId}'.");
        }
        return habits;
    }

    public async Task<int> GetTotalHabitsCountAsync(Guid userId)
    {
        return await _dbSet.CountAsync(h => h.UserId == userId);
    }

    public async Task<double> GetCompletionPercentageForDateAsync(Guid userId, DateTime date)
    {
        var habits = await GetHabitsByUserIdAsync(userId);
        if (!habits.Any())
            return 0;

        var total = habits.Count();
        var completedCount = 0;

        foreach (var habit in habits)
        {
            var completion = await _habitTrackerContext.HabitCompletions
                .Where(c => c.HabitId == habit.Id && c.DateCompleted.Date == date.Date)
                .FirstOrDefaultAsync();

            if (completion != null)
            {
                completedCount++;
            }
        }

        return completedCount / total * 100;
    }

    public async Task<int> GetStreakCountAsync(Guid habitId)
    {
        var completions = await _habitTrackerContext.HabitCompletions
            .Where(c => c.HabitId == habitId)
            .OrderByDescending(c => c.DateCompleted)
            .ToListAsync();

        if (!completions.Any())
            throw new NotFoundException($"No completions found for habit with ID '{habitId}'.");

        int streak = 0;
        DateTime CurrentDate = DateTime.Today;

        foreach (var completion in completions)
        {
            if (completion.DateCompleted.Date == CurrentDate.Date)
            {
                streak++;
                CurrentDate = CurrentDate.AddDays(-1);
            }
            else if (completion.DateCompleted.Date < CurrentDate.Date)
            {
                break;
            }
        }
        return streak;
    }
    
    public async Task<Habit> GetByIdAsync(Guid habitId)
    {
        var habit = await _dbSet.Include(h => h.Completions)
            .FirstOrDefaultAsync(h => h.Id == habitId);

        if (habit == null)
        {
            throw new NotFoundException($"Habit with ID '{habitId}' not found.");
        }

        return habit;
    }
}
