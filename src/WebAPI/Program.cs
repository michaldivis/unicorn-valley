using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using UnicornValley.Domain.Abstractions;
using UnicornValley.Domain.Repositories;
using UnicornValley.Infrastructure.Repositories;
using UnicornValley.WebAPI.SeedData;
using UnicornValley.WebAPI.Services;
using UnicornValley.WebAPI.Utils;

Log.Logger = CreateStartupLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    AddLogging(builder);
    AddDatabase(builder);
    AddServices(builder);
    AddFrameworks(builder);
    AddVersionedDocs(builder);

    var app = builder.Build();

    ConfigureApp(app);
    PrepareApp(app);

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

static Serilog.Core.Logger CreateStartupLogger()
{
    return new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();
}

static void AddLogging(WebApplicationBuilder builder)
{
    builder.Logging.ClearProviders();
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    builder.Logging.AddSerilog(logger);
}

static void AddDatabase(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("InMemoryDb"));
}

static void AddServices(WebApplicationBuilder builder)
{
    //api-specific services
    builder.Services.AddSingleton<IResultHandler, ResultHandler>();
    builder.Services.AddSingleton<IEmailService, EmailService>();

    //infrastructure services
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IMeetingRepository, MeetingRepository>();
    builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
    builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();

    builder.Services.AddScoped<IReadOnlyUserRepository, ReadOnlyUserRepository>();
    builder.Services.AddScoped<IReadOnlyMeetingRepository, ReadOnlyMeetingRepository>();
    builder.Services.AddScoped<IReadOnlyInvitationRepository, ReadOnlyInvitationRepository>();
    builder.Services.AddScoped<IReadOnlyAttendeeRepository, ReadOnlyAttendeeRepository>();
}

static void AddFrameworks(WebApplicationBuilder builder)
{
    builder.Services.AddMediatR(typeof(UnicornValley.Application.AssemblyMarker));
    builder.Services.AddFastEndpoints(o =>
    {
        o.IncludeAbstractValidators = true;
    });
}

static void AddVersionedDocs(WebApplicationBuilder builder)
{
    builder.Services.AddSwaggerDoc(s =>
    {
        s.DocumentName = "Initial Release";
        s.Title = "Unicorn Valley API";
        s.Version = "v1.0";
    });
    builder.Services.AddSwaggerDoc(maxEndpointVersion: 1, settings: s =>
    {
        s.DocumentName = "Release 1.0";
        s.Title = "Unicorn Valley API";
        s.Version = "v1.0";
    });
    builder.Services.AddSwaggerDoc(maxEndpointVersion: 2, settings: s =>
    {
        s.DocumentName = "Release 2.0";
        s.Title = "Unicorn Valley API";
        s.Version = "v2.0";
    });
}

static void ConfigureApp(WebApplication app)
{
    app.UseHttpsRedirection();

    app.UseFastEndpoints(c =>
    {
        c.Versioning.Prefix = "v";

        //configure custom validation failure response
        //there will soon be a cleaner way to access the HttpContext inside the error response builder, see https://github.com/FastEndpoints/FastEndpoints/issues/220
        var ctxAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
        c.Errors.ResponseBuilder = (failures, status) => ResponseUtils.CreateValidationProblemDetails(failures, ctxAccessor.HttpContext, status);
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseOpenApi();
        app.UseSwaggerUi3(s => s.ConfigureDefaults());
    }
}

static void PrepareApp(WebApplication app)
{
    SeedDataInitializer.AddSeedData(app);
}