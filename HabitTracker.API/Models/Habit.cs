using HabitTacker.Models;

public class Habit
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public HabitFrequency Frequency { get; set; }
    public int? CustomFrequency { get; set; } 
    public DateTime StartDate { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public ICollection<HabitCompletion>? Completions { get; set; }
}
