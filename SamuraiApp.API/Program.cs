using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<SamuraiContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString(nameof(SamuraiContext)))
        .EnableSensitiveDataLogging(false)
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
