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
            return new List<Habit>();
        }
        return habits;
    }

    public async Task<int> GetTotalHabitsCountAsync(Guid userId)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet.CountAsync(h => h.UserId == userId && h.StartDate <= today);
    }

    public async Task<double> GetCompletionPercentageForDateAsync(Guid userId, DateTime date)
    {
        var habits = await _dbSet
            .Where(h => h.UserId == userId && h.StartDate.Date <= date.Date)
            .ToListAsync();

        if (!habits.Any())
            return 0;

        var total = habits.Count();
        var completedCount = 0;

        foreach (var habit in habits)
        {
            if (habit.Frequency == HabitFrequency.Weekly)
            {
                var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
                var weekStart = date.Date.AddDays(-1 * diff);
                var weekEnd = weekStart.AddDays(6);

                var completion = await _habitTrackerContext.HabitCompletions
                    .Where(c => c.HabitId == habit.Id && c.DateCompleted.Date >= weekStart && c.DateCompleted.Date <= weekEnd)
                    .FirstOrDefaultAsync();

                if (completion != null)
                {
                    completedCount++;
                }
            }
            else if (habit.Frequency == HabitFrequency.Custom && habit.CustomFrequency > 0)
            {
                int interval = habit.CustomFrequency ?? 0;

                var daysSinceStart = (date.Date - habit.StartDate.Date).Days;
                var intervalsPassed = daysSinceStart / interval;
                var intervalStart = habit.StartDate.Date.AddDays(intervalsPassed * interval);
                var intervalEnd = intervalStart.AddDays(interval - 1);

                var completion = await _habitTrackerContext.HabitCompletions
                    .Where(c => c.HabitId == habit.Id && c.DateCompleted.Date >= intervalStart && c.DateCompleted.Date <= intervalEnd)
                    .FirstOrDefaultAsync();

                if (completion != null)
                {
                    completedCount++;
                }
            }
            else
            {
                var completion = await _habitTrackerContext.HabitCompletions
                    .Where(c => c.HabitId == habit.Id && c.DateCompleted.Date == date.Date)
                    .FirstOrDefaultAsync();

                if (completion != null)
                {
                    completedCount++;
                }
            }
        }

        return (double)completedCount / total * 100;
    }

    public async Task<int> GetStreakCountAsync(Guid habitId)
    {
        var habit = await _dbSet.FirstOrDefaultAsync(h => h.Id == habitId);
        if (habit == null)
            throw new NotFoundException($"Habit with ID '{habitId}' not found.");

        var completions = await _habitTrackerContext.HabitCompletions
            .Where(c => c.HabitId == habitId)
            .OrderByDescending(c => c.DateCompleted)
            .ToListAsync();

        if (!completions.Any())
            throw new NotFoundException($"No completions found for habit with ID '{habitId}'.");

        int streak = 0;

        if (habit.Frequency == HabitFrequency.Weekly)
        {
            DateTime weekStart = DateTime.Today.AddDays(-1 * ((7 + (DateTime.Today.DayOfWeek - DayOfWeek.Monday)) % 7));
            DateTime weekEnd = weekStart.AddDays(6);

            while (true)
            {
                bool completedThisWeek = completions
                    .Any(c => c.DateCompleted.Date >= weekStart && c.DateCompleted.Date <= weekEnd);

                if (completedThisWeek)
                {
                    streak++;
                    weekStart = weekStart.AddDays(-7);
                    weekEnd = weekEnd.AddDays(-7);
                }
                else
                {
                    break;
                }
            }
        }
        else if (habit.Frequency == HabitFrequency.Custom && habit.CustomFrequency > 0)
        {
            int interval = habit.CustomFrequency ?? 0;

            TimeZoneInfo ist = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime start = TimeZoneInfo.ConvertTimeFromUtc(habit.StartDate.ToUniversalTime(), ist).Date;

            DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ist).Date;

            Console.WriteLine($"Habit Start = {start}, Today = {today}");

            int intervalsPassed = (today - start).Days / interval;
            streak = 0;

            for (int i = intervalsPassed; i >= 0; i--)
            {
                DateTime intervalStart = start.AddDays(i * interval);
                DateTime intervalEnd = intervalStart.AddDays(interval - 1);

                if (i == intervalsPassed && intervalEnd > today)
                    intervalEnd = today;

                Console.WriteLine($"i = {i}, intervalStart = {intervalStart:yyyy-MM-dd}, intervalEnd = {intervalEnd:yyyy-MM-dd}");

                bool completedThisInterval = completions
                    .Any(c => c.DateCompleted.Date >= intervalStart &&
                              c.DateCompleted.Date <= intervalEnd);

                if (completedThisInterval)
                {
                    streak++;
                }
                else
                {
                    break;
                }
            }
        }

        else
        {
            DateTime currentDate = DateTime.Today;
            foreach (var completion in completions)
            {
                if (completion.DateCompleted.Date == currentDate.Date)
                {
                    streak++;
                    currentDate = currentDate.AddDays(-1);
                }
                else if (completion.DateCompleted.Date < currentDate.Date)
                {
                    break;
                }
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
