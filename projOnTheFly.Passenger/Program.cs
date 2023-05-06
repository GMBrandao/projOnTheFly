using Microsoft.Extensions.Options;
using projOnTheFly.Passenger.Controllers;
using projOnTheFly.Passenger.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Configuration Singleton and AppSettings parameters.

builder.Services.Configure<ProjOnTheFlyPassengerSettings>(builder.Configuration.GetSection("ProjOnTheFlyPassengerSettings"));
builder.Services.AddSingleton(s => s.GetRequiredService<IOptions<ProjOnTheFlyPassengerSettings>>().Value);
builder.Services.AddSingleton<PassengerService>();



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
