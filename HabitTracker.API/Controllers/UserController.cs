
using HabitTacker.Models;
using HabitTacker.Models.DTO;
using HabitTracker.Interfaces.Services;
using HabitTracker.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HabitTacker.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistrationDto)
    {
        if (userRegistrationDto == null)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("User registration data is required."));
        }

        var user = new User
        {
            Username = userRegistrationDto.Username,
            Email = userRegistrationDto.Email,
            Password = userRegistrationDto.Password
        };

        var createdUser = await _userService.AddUserAsync(user);
        if (createdUser == null)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("User registration failed."));
        }

        return Ok(ApiResponse<User>.SuccessResponse(createdUser, "User registered successfully."));
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid user ID."));
        }
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound(ApiResponse<string>.ErrorResponse("User not found."));
        }
        return Ok(ApiResponse<User>.SuccessResponse(user, "User retrieved successfully."));
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Username is required."));
        }
        var user = await _userService.GetUserByUsernameAsync(username);
        if (user == null)
        {
            return NotFound(ApiResponse<string>.ErrorResponse("User not found."));
        }
        return Ok(ApiResponse<User>.SuccessResponse(user, "User retrieved successfully."));
    }
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Email is required."));
        }
        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
        {
            return NotFound(ApiResponse<string>.ErrorResponse("User not found."));
        }
        return Ok(ApiResponse<User>.SuccessResponse(user, "User retrieved successfully."));
    }

    [HttpGet("{userId}/habits")]
    public async Task<IActionResult> GetUserWithHabits(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid user ID."));
        }
        var user = await _userService.GetUserWithHabitsAsync(userId);
        if (user == null)
        {
            return NotFound(ApiResponse<string>.ErrorResponse("User not found."));
        }
        return Ok(ApiResponse<User>.SuccessResponse(user, "User with habits retrieved successfully."));
    }
}