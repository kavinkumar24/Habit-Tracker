using HabitTacker.Contexts;
using HabitTacker.Exceptions;
using HabitTacker.Interfaces.Repositories;
using HabitTacker.Repositories;
using HabitTacker.Services;
using HabitTracker.Interfaces.Services;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using HabitTracker.API.Helpers;
using System.Threading.RateLimiting;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();


#region  API Versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});
#endregion

#region DB Context
builder.Services.AddDbContext<HabitTrackerContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion


#region Repositories
builder.Services.AddScoped<IHabitRepository, HabitRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHabitCompletionRepository, HabitCompletionRepository>();
#endregion

#region Services
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHabitCompletionService, HabitCompletionService>();
#endregion


#region  Cors
var allowedOrigins = new string[] { "http://localhost:4200" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
#endregion


#region Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var user = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";
        return RateLimitPartition.GetTokenBucketLimiter(user, _ =>
        {
            return new TokenBucketRateLimiterOptions
            {
                TokenLimit = 100,
                TokensPerPeriod = 1000,
                ReplenishmentPeriod = TimeSpan.FromHours(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
            };
        });
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});
#endregion

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AngularCorsPolicy");
app.MapControllers();

app.UseHttpsRedirection();

#region Middleware
app.UseMiddleware<ExceptionMiddleware>();
#endregion


app.Run();
