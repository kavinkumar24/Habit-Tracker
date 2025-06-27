
using HabitTacker.Models.DTO;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace HabitTacker.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class HabitCompletionController : ControllerBase
{
    private readonly IHabitCompletionService _habitCompletionService;

    public HabitCompletionController(IHabitCompletionService habitCompletionService)
    {
        _habitCompletionService = habitCompletionService;
    }

    [HttpPost("complete")]
    public async Task<IActionResult> CompleteHabit([FromBody] HabitTrackingStatusDto completionDto)
    {
        if (completionDto == null || completionDto.HabitId == Guid.Empty)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid habit completion data."));
        }

        var result = await _habitCompletionService.MarkAsCompletedAsync(completionDto.HabitId, completionDto.DateCompleted);
        if (!result)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Habit completion failed."));
        }

        return Ok(ApiResponse<string>.SuccessResponse("Habit completed successfully."));
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveCompletion([FromBody] HabitTrackingStatusDto habitCancelDto)
    {
        if (habitCancelDto == null || habitCancelDto.HabitId == Guid.Empty)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid habit completion data."));
        }
        var result = await _habitCompletionService.RemoveCompletionAsync(habitCancelDto.HabitId, habitCancelDto.DateCompleted);
        if (!result)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Habit completion removal failed."));
        }
        return Ok(ApiResponse<string>.SuccessResponse("Habit completion removed successfully."));
    }
}
