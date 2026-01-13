using CinemaLite.Application.Extensions.Auth;
using CinemaLite.Application.Extensions.Common;
using CinemaLite.Infrastructure.Database.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.RegisterDatabase(builder.Configuration);
builder.Services.AddDependencyInjectionRegistrationService();
builder.Services.AddJwtBearerConfiguration(builder.Configuration);
builder.Services.AddMediatorRegistrationService();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.AddRolesToDatabase();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();