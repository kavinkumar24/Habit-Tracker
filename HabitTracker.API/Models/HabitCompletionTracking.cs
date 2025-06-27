public class HabitCompletion
{
    public Guid Id { get; set; }
    public Guid HabitId { get; set; }
    public DateTime DateCompleted { get; set; }
    public Habit? Habit { get; set; }
}
