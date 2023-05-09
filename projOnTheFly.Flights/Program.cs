using Microsoft.Extensions.Options;
using projOnTheFly.Flights.Config;
using projOnTheFly.Flights.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuration Scoped and AppSettings parameters.


builder.Services.Configure<ProjOnTheFlyFlightSettings>(builder.Configuration.GetSection("ProjOnTheFlyFlightSettings"));
builder.Services.AddScoped<IProjOnTheFlyFlightSettings>(s => s.GetRequiredService<IOptions<ProjOnTheFlyFlightSettings>>().Value);
builder.Services.AddScoped<FlightService>();




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
