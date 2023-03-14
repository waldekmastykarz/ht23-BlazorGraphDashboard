using Microsoft.AspNetCore.Components;

namespace MyDailyDashboard.Components;

public partial class GraphContentBox :ComponentBase
{
    [Parameter] public string Title { get; set; }

    [Parameter] public string Value { get; set; }
    
    [Parameter] public string Icon { get; set; }
    
}