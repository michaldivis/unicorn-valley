using Bogus;
using UnicornValley.Domain.Entities;
using UnicornValley.Domain.Enums;
using UnicornValley.Domain.ValueObjects;

namespace UnicornValley.WebAPI.SeedData;

public static class SeedDataInitializer
{
    public static void AddSeedData(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        using var scope = app.Services.CreateScope();
        var db = scope.Resolve<AppDbContext>();

        Randomizer.Seed = new Random(123456);

        var users = new Faker<User>()
            .UsePrivateConstructor()
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .RuleFor(a => a.Username, f => Username.From(f.Person.Email))
            .Generate(3);

        db.AddRange(users);
        db.SaveChanges();

        var meetings = new Faker<Meeting>()
            .UsePrivateConstructor()
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .RuleFor(a => a.Creator, f => f.PickRandom(users))
            .RuleFor(a => a.Type, f => f.PickRandom<MeetingType>())
            .RuleFor(a => a.ScheduledAtUtc, f => f.Date.Future())
            .RuleFor(a => a.Name, f => f.Name.JobTitle())
            .RuleFor(a => a.Location, f => f.Address.FullAddress())
            .RuleFor(a => a.MaximumNumberOfAttendees, f => f.Random.Number(1, 500))
            .RuleFor(a => a.InvitationsExpireAtUtc, (f, m) => f.Date.Between(DateTime.UtcNow, m.ScheduledAtUtc))
            .Generate(3);

        db.AddRange(meetings);
        db.SaveChanges();
    }
}
