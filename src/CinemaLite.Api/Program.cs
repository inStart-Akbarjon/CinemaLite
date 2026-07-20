using CinemaLite.Api.Extensions;
using CinemaLite.Api.Middlewares;
using CinemaLite.Application.Common.Stripe;
using CinemaLite.Application.Extensions.Auth;
using CinemaLite.Application.Extensions.Common;
using CinemaLite.Infrastructure.BackgroundServices;
using CinemaLite.Infrastructure.Database.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.RegisterDatabase();
builder.AddDependencyInjectionRegistrationService();
builder.AddJwtBearerConfiguration();
builder.AddMediatorRegistrationService();
builder.AddRedisLock();
builder.AddRedisCache();
builder.AddRabbitMq();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddHostedService<ExpireSessionWorker>();
builder.Services.AddHostedService<ExpireTopMoviesWorker>();
builder.Services.AddHostedService<ExpireTicketWorker>();
builder.Services.AddHostedService<UserReminderWorker>();
builder.Services.AddHostedService<TicketPricingWorker>();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<StripeSettings>>().Value);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.AddRolesToDatabase();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();