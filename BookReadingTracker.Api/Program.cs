using BookReadingTracker.Api.Services;
using BookReadingTracker.Database.AppDbContextModels;
using BookReadingTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookReadingService, BookReadingService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

SeedAdmin(app);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

static void SeedAdmin(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var admin = db.Users.FirstOrDefault(x => x.Email == "admin@booktracker.com");
    if (admin is null)
    {
        db.Users.Add(new User
        {
            UserName = "Admin",
            Email = "admin@booktracker.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            RoleName = "Admin",
            CreatedBy = "system",
            CreatedDate = DateTime.UtcNow
        });
    }
    else
    {
        admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
    }
    db.SaveChanges();
}
