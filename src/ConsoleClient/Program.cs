using ApiClient;
using OneOf;
using Refit;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

var api = RestService.For<IUnicornValleyApi>("https://localhost:7135");

Console.WriteLine("Running demo...");

PrintHeader("Create user");
var createdUserResult = await HandleRequestAsync(() => api.CreateUser(new UnicornValley.Application.Users.Commands.CreateUserCommand { Username = "gandalf@magic.com" }));
var createdUser = createdUserResult.AsT0;

PrintHeader("Get all users");
var allUsersResult = await HandleRequestAsync(() => api.GetAllUsers());
var anotherUser = allUsersResult.AsT0.First();

PrintHeader("Get user - with valid ID");
_ = await HandleRequestAsync(() => api.GetUser(createdUser.Id));

PrintHeader("Get user - with invalid ID");
_ = await HandleRequestAsync(() => api.GetUser(Guid.Empty));


PrintHeader("Errors - single");
_ = await HandleRequestAsync(() => api.GetSingleError());

PrintHeader("Errors - multiple");
_ = await HandleRequestAsync(() => api.GetMultipleErrors());


PrintHeader("Create meeting");
var createdMeetingResult = await HandleRequestAsync(() => api.CreateMeeting(new UnicornValley.Application.Meetings.Commands.CreateMeetingCommand
{
    CreatorId = createdUser.Id,
    Name = "Building thall guitar tones 101",
    Location = "Stockholm, Sweden",
    ScheduledAtUtc = DateTime.UtcNow.AddDays(10),
    Type = UnicornValley.Domain.Enums.MeetingType.WithLimitedNumberOfAttendees,
    MaximumNumberOfAttendees = 25
}));
var createdMeeting = createdMeetingResult.AsT0;

PrintHeader("Get all meetings");
_ = await HandleRequestAsync(() => api.GetAllMeetings());


PrintHeader("Send invitation");
var sentInvitationResult = await HandleRequestAsync(() => api.SendInvitation(new UnicornValley.Application.Invitations.Commands.SendInvitationCommand
{
    MeetingId = createdMeeting.Id,
    UserId = anotherUser.Id
}));
var sentInvitation = sentInvitationResult.AsT0;

PrintHeader("Accept invitation");
_ = await HandleRequestAsync(() => api.AcceptInvitation(new UnicornValley.Application.Invitations.Commands.AcceptInvitationCommand
{
    InvitationId = sentInvitation.Id
}));

static async Task<OneOf<TResponse, ApiProblemDetails?>> HandleRequestAsync<TResponse>(Func<Task<TResponse>> apiCall)
{
    try
    {
        var result = await apiCall.Invoke();
        PrintData(result, true);
        return result;
    }
    catch (ApiException ex)
    {
        var error = await ex.GetContentAsAsync<ApiProblemDetails>();

        if (error is null)
        {
            var unknownError = ApiProblemDetails.Unknown(ex.RequestMessage.RequestUri?.AbsolutePath ?? "Unknown", (int)ex.StatusCode);
            PrintData(unknownError, false);
            return unknownError;
        }

        PrintData(error, false);
        return error;
    }
}

static void PrintHeader(string text)
{
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine(text.ToUpper());
    Console.WriteLine("-----------------------------------");
}

static void PrintData(object? data, bool success)
{
    if (data is null)
    {
        return;
    }

    var originalColor = Console.ForegroundColor;
    Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;

    Console.WriteLine(success ? "Success response:" : "Error response:");
    var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
    Console.WriteLine(json);

    Console.ForegroundColor = originalColor;
}