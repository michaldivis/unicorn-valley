﻿using System.Runtime.CompilerServices;

namespace UnicornValley.Domain.Common;

public static class DomainErrors
{
    public static class Common
    {
        public static readonly DomainError NotFoundById = Create("Entity with this ID not found.");

        private static DomainError Create(string message, [CallerMemberName] string code = "")
        {
            return new($"{nameof(Common)}.{code}", message);
        }
    }

	public static class Meeting
	{
		public static readonly DomainError InvitingCreator = Create( "Can't send invitation to the meeting creator.");
        public static readonly DomainError AlreadyPassed = Create("Can't send invitation for a meeting in the past.");
        public static readonly DomainError MaximumNumberOfAttendeesMissing = Create("Maximum number of attendees is missing.");
        public static readonly DomainError InvitationValidBeforeInHoursMissing = Create("Invitation valid before in hours is missing.");
        public static readonly DomainError InvitationExpired = Create("Invitation is expired.");
        public static readonly DomainError InvitationAlreadyAccepted = Create("Invitation is already accepted.");
        public static readonly DomainError InvitationAlreadyExists = Create("Invitation is already exists.");

        private static DomainError Create(string message, [CallerMemberName] string code = "")
		{
            return new($"{nameof(Meeting)}.{code}", message);
        }
    }

    public static class User
    {
        public static readonly DomainError UsernameAlreadyExists = Create("A user with this username already exists.");
        public static readonly DomainError InvalidUsername = Create("Username not in a valid format.");

        private static DomainError Create(string message, [CallerMemberName] string code = "")
        {
            return new($"{nameof(User)}.{code}", message);
        }
    }
}
