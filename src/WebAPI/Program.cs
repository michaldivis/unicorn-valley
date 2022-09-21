using FastEndpoints;
using MediatR;
using Serilog.Events;
using Serilog;
using UnicornValley.Domain.Abstractions;
using UnicornValley.Domain.Repositories;
using UnicornValley.Infrastructure;
using UnicornValley.Infrastructure.Repositories;
using UnicornValley.WebAPI.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    //configure logging
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog();

    //TODO configure DB context
    builder.Services.AddDbContext<AppDbContext>();

    //register api-specific services
    builder.Services.AddSingleton<IErrorHandler, ErrorHandler>();
    builder.Services.AddSingleton<IEmailService, EmailService>();

    //register infrastructure
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IMeetingRepository, MeetingRepository>();
    builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
    builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();

    builder.Services.AddMediatR(typeof(UnicornValley.Application.Marker));
    builder.Services.AddFastEndpoints();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseFastEndpoints();

    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}