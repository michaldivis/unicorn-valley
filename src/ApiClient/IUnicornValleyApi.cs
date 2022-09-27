using ApiClient.Models;
using Refit;
using UnicornValley.Application.Invitations.Commands;
using UnicornValley.Application.Meetings.Commands;
using UnicornValley.Application.Users.Commands;

namespace ApiClient;
public interface IUnicornValleyApi
{
    [Get("/users/{UserId}")]
    Task<UserModel> GetUser(Guid userId);

    [Get("/users/all")]
    Task<List<UserModel>> GetAllUsers();

    [Post("/users/create")]
    Task<UserModel> CreateUser(CreateUserCommand req);


    [Get("/demo/single-error")]
    Task<ApiProblemDetails> GetSingleError();

    [Get("/demo/multiple-errors-result")]
    Task<ApiProblemDetails> GetMultipleErrors();


    [Get("/meetings/{MeetingId}")]
    Task<MeetingModel> GetMeeting(Guid meetingId);

    [Get("/meetings/all")]
    Task<List<MeetingModel>> GetAllMeetings();

    [Post("/meetings/create")]
    Task<MeetingModel> CreateMeeting(CreateMeetingCommand req);


    [Get("/invitations/{InvitationId}")]
    Task<InvitationModel> GetInvitation(Guid invitationId);

    [Get("/invitations/all")]
    Task<List<InvitationModel>> GetAllInvitations();

    [Post("/invitations/send")]
    Task<InvitationModel> SendInvitation(SendInvitationCommand req);

    [Post("/invitations/accept")]
    Task<AttendeeModel> AcceptInvitation(AcceptInvitationCommand req);
}
