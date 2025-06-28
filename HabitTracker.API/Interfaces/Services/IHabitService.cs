using HabitTacker.Models.DTO;
using HabitTracker.API.Models.DTO;

namespace HabitTracker.Interfaces.Services;

public interface IHabitService
{
    Task<HabitResponseDto> AddHabitAsync(HabitAddDto habit);
    Task<IEnumerable<HabitResponseDto>> GetHabitsByUserIdAsync(Guid userId);
    Task<HabitResponseDto?> GetHabitByIdAsync(Guid habitId);
    Task<Habit?> UpdateHabitAsync(Guid habitId, HabitUpdateDto updatedHabitDto);
    Task DeleteHabitAsync(Guid habitId);
    Task<double> GetCompletionPercentageForDateAsync(Guid userId, DateTime date);
    Task<int> GetStreakCountAsync(Guid habitId);
}