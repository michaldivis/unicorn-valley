using Microsoft.EntityFrameworkCore;
using UnicornValley.Domain.Entities;

namespace UnicornValley.Infrastructure;
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<Attendee> Attendees { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public AppDbContext()
	{
	}

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
