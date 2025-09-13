namespace RiskMgmt.Web.Hubs;
//{
//    public class PriceHub
//    {
//    }
//}

// Hubs/PriceHub.cs
using Microsoft.AspNetCore.SignalR;

public class PriceHub : Hub
{
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}

