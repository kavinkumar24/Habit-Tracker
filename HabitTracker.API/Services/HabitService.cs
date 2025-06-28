
using AutoMapper;
using HabitTacker.Interfaces.Repositories;
using HabitTacker.Models.DTO;
using HabitTracker.API.Models.DTO;
using HabitTracker.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HabitTacker.Services;

public class HabitService : IHabitService
{
    private readonly IHabitRepository _habitRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public HabitService(IHabitRepository habitRepository, IUserRepository userRepository, IMapper mapper)
    {
        _habitRepository = habitRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task<HabitResponseDto> AddHabitAsync(HabitAddDto habit)
    {
        if (habit == null)
        {
            throw new ArgumentNullException(nameof(habit), "Habit cannot be null");
        }

        if (habit.UserId == Guid.Empty)
                throw new ArgumentException("UserId must be provided.", nameof(habit.UserId));

        var userExists = await _userRepository.Get(habit.UserId);

        if (userExists == null)
            throw new ArgumentException("User does not exist.", nameof(habit.UserId));

        if (string.IsNullOrWhiteSpace(habit.Title))
            throw new ArgumentException("Title must not be empty.", nameof(habit.Title));

        if (habit.StartDate < DateTime.UtcNow.Date)
            throw new ArgumentException("StartDate cannot be in the past.", nameof(habit.StartDate));
        
        var existingHabits = await _habitRepository.GetHabitsByUserIdAsync(habit.UserId);

        if (existingHabits.Any(h => h.Title.Equals(habit.Title, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("A habit with this title already exists for the user.");
        }

        if (existingHabits.Any(h => h.StartDate == habit.StartDate))
        {
            throw new InvalidOperationException("A habit with this start date and time already exists for the user.");
        }

         var newHabit = _mapper.Map<Habit>(habit);
         var createdHabit = await _habitRepository.Add(newHabit);
         return _mapper.Map<HabitResponseDto>(createdHabit);

    }

    public Task DeleteHabitAsync(Guid habitId)
    {
        if (habitId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(habitId), "Habit ID cannot be empty");
        }

        return _habitRepository.Delete(habitId);
    }

    public Task<double> GetCompletionPercentageForDateAsync(Guid userId, DateTime date)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(userId), "User ID cannot be empty");
        }
        if(date == default)
        {
            throw new ArgumentNullException(nameof(date), "Date cannot be default value");
        }

        return _habitRepository.GetCompletionPercentageForDateAsync(userId, date);
    }

    public async Task<HabitResponseDto?> GetHabitByIdAsync(Guid habitId)
    {
        if (habitId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(habitId), "Habit ID cannot be empty");
        }

        var habit = await _habitRepository.GetByIdAsync(habitId);
        if (habit == null)
        {
            return null;
        }

        return _mapper.Map<HabitResponseDto>(habit);
    }

    public async Task<IEnumerable<HabitResponseDto>> GetHabitsByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(userId), "User ID cannot be empty");
        }
        
        var habits = await _habitRepository.GetHabitsByUserIdAsync(userId);
    return habits.Select(h => _mapper.Map<HabitResponseDto>(h));
    }

    public Task<int> GetStreakCountAsync(Guid habitId)
    {
        if (habitId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(habitId), "Habit ID cannot be empty");
        }
        
        return _habitRepository.GetStreakCountAsync(habitId);
    }

   public async Task<Habit?> UpdateHabitAsync(Guid habitId, HabitUpdateDto dto)
{
    if (habitId == Guid.Empty)
        throw new ArgumentException("Habit ID cannot be empty", nameof(habitId));

    if (dto == null)
        throw new ArgumentNullException(nameof(dto));

    var existing = await _habitRepository.Get(habitId);
    if (existing == null)
        return null;

    if (!string.IsNullOrWhiteSpace(dto.Title))
        existing.Title = dto.Title;

    if (!string.IsNullOrWhiteSpace(dto.Description))
        existing.Description = dto.Description;

    if (dto.Frequency.HasValue)
    {
        if (!Enum.IsDefined(typeof(HabitFrequency), dto.Frequency.Value))
            throw new ArgumentException($"Invalid frequency value: {dto.Frequency}", nameof(dto.Frequency));

        existing.Frequency = dto.Frequency.Value;
    }


    if (dto.CustomFrequency.HasValue)
        existing.CustomFrequency = dto.CustomFrequency.Value;

    if (dto.StartDate.HasValue)
        existing.StartDate = dto.StartDate.Value;

    return await _habitRepository.Update(habitId, existing);
}



}

public class AddHabitAsync
{
}