using BookTracker.Api.Wiring;

var builder = WebApplication.CreateBuilder(args);
builder.AddBookTracker();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseBookTracker();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

public partial class Program;