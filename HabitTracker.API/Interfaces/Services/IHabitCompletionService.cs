
namespace HabitTracker.Services;

public interface IHabitCompletionService
{
    Task<bool> MarkAsCompletedAsync(Guid habitId, DateTime date);
    Task<bool> RemoveCompletionAsync(Guid habitId, DateTime date);
}