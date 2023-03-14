using Microsoft.Graph;
using Microsoft.Identity.Web;
using MyDailyDashboard.Interfaces;

namespace MyDailyDashboard.Data;

public class GraphService:IGraphService
{
    private readonly GraphServiceClient _graphServiceClient;
    private readonly MicrosoftIdentityConsentAndConditionalAccessHandler _consentHandler;

    
    public GraphService(GraphServiceClient graphServiceClient, MicrosoftIdentityConsentAndConditionalAccessHandler consentHandler)
    {
        _graphServiceClient = graphServiceClient;
        _consentHandler = consentHandler;
    }
    
    public async Task<IUserCalendarViewCollectionPage> GetUserCalendarViewCollectionPage()
    {
        try
        {
            var startTodayDateTime = DateTime.Now.AddMilliseconds(1).ToString("o");        
            var endTodayDateTime = DateTime.Today.AddDays(1).ToString("o");
            var queryOptions = new List<QueryOption>()
            {
                new QueryOption("startDateTime", startTodayDateTime),
                new QueryOption("endDateTime", endTodayDateTime)
            };
            return await _graphServiceClient.Me.CalendarView
                .Request(queryOptions)
                .GetAsync();
        }
        catch (Exception e)
        {
            _consentHandler.HandleException(e);
            return new UserCalendarViewCollectionPage();
        }
    }

    public int GetAppointmentsForToday(IUserCalendarViewCollectionPage calendarViewCollectionPage)
    {
        return calendarViewCollectionPage.Where(x => x.IsOnlineMeeting is false).ToList().Count;
    }
    
    public int GetOnlineMeetingsForToday(IUserCalendarViewCollectionPage calendarViewCollectionPage)
    {
        return calendarViewCollectionPage.Where(x => x.IsOnlineMeeting is true).ToList().Count;
    }
    
    public async Task<int> GetUnreadOutlookMessages()
    {
        try
        {
            var unreadMessages = await _graphServiceClient.Me.MailFolders["Inbox"].Messages
                .Request()
                .Filter($"isRead eq false")
                .GetAsync();
            return unreadMessages?.Count ?? 0;
        }
        catch (Exception e)
        {
            _consentHandler.HandleException(e);
            return 0;
        }
       
    }
    
    public async Task<string> GetDriveInformation()
    {
        try
        {
            var drive = await _graphServiceClient.Me.Drive
                .Request()
                .GetAsync();

            if (drive.Quota.Remaining != null) return DataSizeFormatterService.Format(drive.Quota.Remaining.Value);
            return "Failed";
        }
        catch (Exception e)
        {
            _consentHandler.HandleException(e);
            return "0";
        }
       
    }
    
    public async Task<int> GetToDoList()
    {
        try
        {
            var todoLists = await _graphServiceClient.Me.Todo.Lists
                .Request()
                .GetAsync();
            return todoLists?.Count ?? 0;
        }
        catch (Exception e)
        {
            _consentHandler.HandleException(e);
            return 0;
        }
       
    }

    public async Task<User> GetCurrentUser()
    {
        try
        {
            return  await _graphServiceClient.Me
                .Request()
                .GetAsync();
        }
        catch (Exception e)
        {
            _consentHandler.HandleException(e);
            return new User(){DisplayName = "Failed to fetch data"};
        }
    }
    
    public async Task<string> GetUserPhoto()
    {
        try
        {
            var photo = await _graphServiceClient.Me.Photo
                .Content
                .Request()
                .GetAsync();
            var image = Convert.ToBase64String((photo as MemoryStream)?.ToArray() ?? Array.Empty<byte>());
            return image;
        }
        catch (Exception e)
        {
            _consentHandler.HandleException(e);
            return "";
        }
    }

    public async Task<int> GetUsersChats()
    {
        try
        {
           var chat = await _graphServiceClient.Me.Chats
               .Request()
               .GetAsync();
           return chat.Count;
        }
        catch (Exception e)
        {
            _consentHandler.HandleException(e);
            return 0;
        }
    }
}