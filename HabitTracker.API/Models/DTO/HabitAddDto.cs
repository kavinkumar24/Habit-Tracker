namespace HabitTacker.Models.DTO;

public class HabitAddDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public HabitFrequency Frequency { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public int? CustomFrequency { get; set; } 
    public Guid UserId { get; set; }

    public string FrequencyText => Frequency.ToString();
}