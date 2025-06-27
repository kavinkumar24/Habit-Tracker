
using HabitTacker.Models.DTO;
using HabitTracker.API.Models.DTO;
using HabitTracker.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HabitTacker.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class HabitController : ControllerBase
{
    private readonly IHabitService _habitService;
    public HabitController(IHabitService habitService)
    {
        _habitService = habitService;
    }

    [HttpPost]
    public async Task<IActionResult> AddHabit([FromBody] HabitAddDto habit)
    {
        if (habit == null)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Habit data is required."));
        }
         var createdHabit = await _habitService.AddHabitAsync(habit);
        if (createdHabit == null)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Habit creation failed."));
        }
        return Ok(ApiResponse<HabitResponseDto>.SuccessResponse(createdHabit, "Habit created successfully."));
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetHabitsByUserId(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid user ID."));
        }
        var habits = await _habitService.GetHabitsByUserIdAsync(userId);
        if (habits == null || !habits.Any())
        {
            return NotFound(ApiResponse<string>.ErrorResponse("No habits found for this user."));
        }
        return Ok(ApiResponse<IEnumerable<HabitResponseDto>>.SuccessResponse(habits, "Habits retrieved successfully."));
    }

    [HttpGet("habit/{habitId}")]
    public async Task<IActionResult> GetHabitById(Guid habitId)
    {
        if (habitId == Guid.Empty)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid habit ID."));
        }
        var habit = await _habitService.GetHabitByIdAsync(habitId);
        if (habit == null)
        {
            return NotFound(ApiResponse<string>.ErrorResponse("Habit not found."));
        }
        return Ok(ApiResponse<Habit>.SuccessResponse(habit, "Habit retrieved successfully."));
    }

    [HttpPut("{habitId}")]
    public async Task<IActionResult> UpdateHabit(Guid habitId, [FromBody] HabitUpdateDto updatedHabit)

    {
        if (habitId == Guid.Empty || updatedHabit == null)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid habit ID or habit data."));
        }
        var updated = await _habitService.UpdateHabitAsync(habitId, updatedHabit);
        if (updated == null)
        {
            return NotFound(ApiResponse<string>.ErrorResponse("Habit not found or update failed."));
        }
        return Ok(ApiResponse<Habit>.SuccessResponse(updated, "Habit updated successfully."));
    }

    [HttpDelete("{habitId}")]
    public async Task<IActionResult> DeleteHabit(Guid habitId)
    {
        if (habitId == Guid.Empty)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid habit ID."));
        }
        await _habitService.DeleteHabitAsync(habitId);
        return Ok(ApiResponse<string>.SuccessResponse("Habit deleted successfully."));
    }

    [HttpGet("completion/{userId}/{date}")]
    public async Task<IActionResult> GetCompletionPercentageForDate(Guid userId, DateTime date)
    {
        if (userId == Guid.Empty || date == default)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid user ID or date."));
        }
        var completionPercentage = await _habitService.GetCompletionPercentageForDateAsync(userId, date);

        return Ok(ApiResponse<double>.SuccessResponse(completionPercentage, "Completion percentage retrieved successfully."));
    }

    [HttpGet("streak/{habitId}")]
    public async Task<IActionResult> GetStreakCount(Guid habitId)
    {
        if (habitId == Guid.Empty)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid habit ID."));
        }
        var streakCount = await _habitService.GetStreakCountAsync(habitId);

        return Ok(ApiResponse<int>.SuccessResponse(streakCount, "Streak count retrieved successfully."));
    }

}