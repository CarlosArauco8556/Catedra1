using Catedra1.src.Data;
using Catedra1.src.Interfaces;
using Catedra1.src.Repository;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();


string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "Data Source=app.db";
builder.Services.AddDbContext<ApplicationDBContext>(opt => opt.UseSqlite(connectionString));
var app = builder.Build();

using ( var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDBContext>();
    DataSeeder.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();