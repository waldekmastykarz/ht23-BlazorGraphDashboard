using Microsoft.Graph;

namespace MyDailyDashboard.Interfaces;

public interface IGraphService
{
    Task<int> GetUnreadOutlookMessages();
    Task<int> GetToDoList();
    Task<User> GetCurrentUser();
    Task<string> GetDriveInformation();
    Task<string> GetUserPhoto();
    Task<IUserCalendarViewCollectionPage> GetUserCalendarViewCollectionPage();
    Task<int> GetUsersChats();
    int GetOnlineMeetingsForToday(IUserCalendarViewCollectionPage calendarViewCollectionPage);
    int GetAppointmentsForToday(IUserCalendarViewCollectionPage calendarViewCollectionPage);
}