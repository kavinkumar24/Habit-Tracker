using AutoMapper;
using HabitTacker.Models.DTO;
using HabitTacker.Models;
using HabitTracker.API.Models.DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Habit, HabitResponseDto>();
        CreateMap<HabitAddDto, Habit>();
    }
}