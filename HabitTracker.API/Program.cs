using HabitTacker.Contexts;
using HabitTacker.Exceptions;
using HabitTacker.Interfaces.Repositories;
using HabitTacker.Repositories;
using HabitTacker.Services;
using HabitTracker.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DB Context
builder.Services.AddDbContext<HabitTrackerContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion


#region Repositories
builder.Services.AddScoped<IHabitRepository, HabitRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
#endregion

#region Services
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<IUserService, UserService>();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Middleware
app.UseMiddleware<ExceptionMiddleware>();
#endregion

app.Run();
