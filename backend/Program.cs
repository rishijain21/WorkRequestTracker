using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkRequestTracker.API.Filters;
using WorkRequestTracker.API.Middleware;
using WorkRequestTracker.Application.Interfaces;
using WorkRequestTracker.Application.Services;
using WorkRequestTracker.Infrastructure.Persistence;
using WorkRequestTracker.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON enum serialization + validation filter
builder.Services.AddControllers(options =>
    options.Filters.Add<ValidationFilter>())
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Use our custom validation filter instead of ASP.NET's default ProblemDetails
builder.Services.Configure<ApiBehaviorOptions>(options =>
    options.SuppressModelStateInvalidFilter = true);

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI registration (scoped = one instance per HTTP request)
builder.Services.AddScoped<IWorkRequestRepository, WorkRequestRepository>();
builder.Services.AddScoped<IWorkRequestService, WorkRequestService>();

// CORS — reads allowed origins from config (supports multiple env: local + Vercel)
var allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>() ?? ["http://localhost:3000"];

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline (order matters)

// Exception middleware must be first in the pipeline
app.UseMiddleware<ExceptionMiddleware>();


app.UseCors();

// Apply EF migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Swagger (always on — useful for live demo)
app.UseSwagger();
app.UseSwaggerUI();


app.MapControllers();

app.Run();
