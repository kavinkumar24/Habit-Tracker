namespace HabitTacker.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public ICollection<Habit>? Habits { get; set; }
}