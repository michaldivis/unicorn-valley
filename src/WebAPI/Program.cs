using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using UnicornValley.Domain.Abstractions;
using UnicornValley.Domain.Repositories;
using UnicornValley.Infrastructure.Repositories;
using UnicornValley.WebAPI.SeedData;
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
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    builder.Logging.AddSerilog(logger);

    //configure DB context
    builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("InMemoryDb"));

    //register api-specific services
    builder.Services.AddSingleton<IResultHandler, ResultHandler>();
    builder.Services.AddSingleton<IEmailService, EmailService>();

    //register infrastructure
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IMeetingRepository, MeetingRepository>();
    builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
    builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();

    builder.Services.AddScoped<IReadOnlyUserRepository, ReadOnlyUserRepository>();
    builder.Services.AddScoped<IReadOnlyMeetingRepository, ReadOnlyMeetingRepository>();
    builder.Services.AddScoped<IReadOnlyInvitationRepository, ReadOnlyInvitationRepository>();
    builder.Services.AddScoped<IReadOnlyAttendeeRepository, ReadOnlyAttendeeRepository>();

    builder.Services.AddMediatR(typeof(UnicornValley.Application.AssemblyMarker));
    builder.Services.AddFastEndpoints();
    builder.Services.AddSwaggerDoc();

    var app = builder.Build();

    app.UseHttpsRedirection();
    app.UseFastEndpoints();

    if (app.Environment.IsDevelopment())
    {
        app.UseOpenApi();
        app.UseSwaggerUi3(s => s.ConfigureDefaults());
    }

    SeedDataInitializer.AddSeedData(app);

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