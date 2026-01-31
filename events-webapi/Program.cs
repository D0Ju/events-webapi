using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using events_webapi.Models;
using events_webapi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppdbContext") ?? throw new InvalidOperationException("Connection string 'AppdbContext' not found.")));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//dodaj servise
builder.Services.AddScoped<IEventApiService, EventApiService>();
builder.Services.AddScoped<IEventTypeApiService, EventTypeApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
