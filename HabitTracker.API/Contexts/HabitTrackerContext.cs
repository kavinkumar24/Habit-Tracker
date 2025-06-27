namespace HabitTacker.Contexts;
using HabitTacker.Models;
using Microsoft.EntityFrameworkCore;

public class HabitTrackerContext : DbContext
{
    public HabitTrackerContext(DbContextOptions<HabitTrackerContext> options) : base(options)
    {
    }

    public DbSet<Habit> Habits { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<HabitCompletion> HabitCompletions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Habit>()
            .HasOne(h => h.User)
            .WithMany(u => u.Habits)
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HabitCompletion>()
            .HasOne(c => c.Habit)
            .WithMany(h => h.Completions)
            .HasForeignKey(c => c.HabitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}