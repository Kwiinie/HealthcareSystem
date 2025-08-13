using Microsoft.AspNetCore.SignalR;

namespace FindingHealthcareSystem.Hubs
{
    public class UpdateHub : Hub
    {
        public async Task NotifyDataUpdate(string typeUpdate,string message)
        {
            await Clients.All.SendAsync(typeUpdate, message);
        }
    }
}
