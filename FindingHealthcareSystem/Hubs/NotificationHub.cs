using Microsoft.AspNetCore.SignalR;


namespace FindingHealthcareSystem.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task BroadcastNotification(string message, string type = "info")
        {
            await Clients.All.SendAsync("ReceiveNotification", new
            {
                message,
                type
            });
        }
    }
}
