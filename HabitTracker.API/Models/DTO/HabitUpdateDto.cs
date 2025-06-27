namespace HabitTracker.API.Models.DTO;
public class HabitUpdateDto
{
    public string? Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public HabitFrequency? Frequency { get; set; }
    public int? CustomFrequency { get; set; }
    public DateTime? StartDate { get; set; }
}
