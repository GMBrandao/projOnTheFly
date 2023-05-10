using Microsoft.Extensions.Options;
using projOnTheFly.Sales.Config;
using projOnTheFly.Sales.Service;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<ProjOnTheFlySaleSettings>(builder.Configuration.GetSection("ProjOnTheFlySaleSettings"));
builder.Services.AddScoped<IProjOnTheFlySaleSettings>(s => s.GetRequiredService<IOptions<ProjOnTheFlySaleSettings>>().Value);
builder.Services.AddScoped<SaleService>();

builder.Services.AddScoped<ConnectionFactory>();

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
