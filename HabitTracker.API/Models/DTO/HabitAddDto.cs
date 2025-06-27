namespace HabitTacker.Models.DTO;

public class HabitAddDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty; 
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
}