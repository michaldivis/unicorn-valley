using Microsoft.EntityFrameworkCore;

namespace UnicornValley.Infrastructure;
public static class IncludeExtensions
{
    public static IQueryable<Meeting> IncludeAll(this IQueryable<Meeting> meetings)
    {
        return meetings
            .Include(a => a.Creator)
            .Include(a => a.Invitations)
            .Include(a => a.Attendees);
    }

    public static IQueryable<User> IncludeAll(this IQueryable<User> users)
    {
        return users;
    }

    public static IQueryable<Invitation> IncludeAll(this IQueryable<Invitation> invitations)
    {
        return invitations;
    }

    public static IQueryable<Attendee> IncludeAll(this IQueryable<Attendee> attendees)
    {
        return attendees;
    }
}
