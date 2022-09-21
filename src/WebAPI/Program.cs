using FastEndpoints;
using MediatR;
using UnicornValley.Domain.Abstractions;
using UnicornValley.Domain.Repositories;
using UnicornValley.Infrastructure;
using UnicornValley.Infrastructure.Repositories;
using UnicornValley.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

//TODO configure DB context
builder.Services.AddDbContext<AppDbContext>();

//register api-specific services
builder.Services.AddScoped<IErrorHandler, ErrorHandler>();
builder.Services.AddScoped<IEmailService, EmailService>();

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