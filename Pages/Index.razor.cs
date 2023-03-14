using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using MyDailyDashboard.Data;
using MyDailyDashboard.Interfaces;

namespace MyDailyDashboard.Pages;

public partial class Index:ComponentBase
{
    [Inject]  public IGraphService GraphService { get; set; }
   
    private User? _user;
    
    private int _currentToDosLists = 0;
    private int _currentUnreadMessages = 0;
    private int _currentMeetingsToday = 0;
    private int _currentTeamsMeetingToday = 0;
    private int _currentChats = 0;
    private string _currentUserPicture = string.Empty;
    private string _remainingDrive = "0";
    
    protected override async Task OnInitializedAsync()
    {
        _user = await GraphService.GetCurrentUser();
        var calendarViews = await GraphService.GetUserCalendarViewCollectionPage();
        _currentUnreadMessages = await GraphService.GetUnreadOutlookMessages();
        _currentToDosLists  = await GraphService.GetToDoList();
        _currentMeetingsToday =  GraphService.GetAppointmentsForToday(calendarViews);
        _currentTeamsMeetingToday = GraphService.GetOnlineMeetingsForToday(calendarViews);
        _currentUserPicture = await GraphService.GetUserPhoto();
        _remainingDrive = await GraphService.GetDriveInformation();
        _currentChats = await GraphService.GetUsersChats();
    }
    
}