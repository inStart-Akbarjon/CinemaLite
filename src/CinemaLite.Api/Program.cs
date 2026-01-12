using CinemaLite.Application.Interfaces.DbContext;
using CinemaLite.Infrastructure.Data;
using CinemaLite.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbConnection(builder.Configuration);
builder.Services.AddScoped<IAppDbContext, AppDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();