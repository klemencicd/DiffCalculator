using Carter;
using DiffCalculatorApi.Repositories;
using DiffCalculatorApi.Repositories.Interfaces;
using DiffCalculatorApi.Services;
using DiffCalculatorApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter();
builder.Services.AddTransient<IDiffCalculator, DiffCalculator>();
builder.Services.AddTransient<IDiffRepository, DiffRepository>();

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

app.MapCarter();

app.Run();

public partial class Program { }