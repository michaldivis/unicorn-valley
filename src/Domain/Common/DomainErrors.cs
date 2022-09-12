using System.Runtime.CompilerServices;

namespace UnicornValley.Domain.Common;

public static class DomainErrors
{
	public static class Meeting
	{
		public static readonly DomainError InvitingCreator = Create( "Can't send invitation to the meeting creator.");
        public static readonly DomainError AlreadyPassed = Create("Can't send invitation for a meeting in the past.");

		private static DomainError Create(string message, [CallerMemberName] string code = "")
		{
            return new($"{nameof(Meeting)}.{code}", message);
        }
    }
}
