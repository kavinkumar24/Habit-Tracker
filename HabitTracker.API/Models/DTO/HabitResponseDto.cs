namespace HabitTracker.API.Models.DTO;
public class HabitResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public HabitFrequency Frequency { get; set; }
    public int? CustomFrequency { get; set; }
    public DateTime StartDate { get; set; }
    public Guid UserId { get; set; }
}