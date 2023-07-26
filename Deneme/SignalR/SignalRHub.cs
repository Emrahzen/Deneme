using Microsoft.AspNetCore.SignalR;
namespace Deneme.SignalR
{
    public class SignalRHub : Hub
    {
        public async override Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Connected", $"{Context.ConnectionId}");
        }

        public void SendMessage(string message)
        {
            try
            {
                if (Clients != null)
                {
                    if (Clients.All != null)
                    {
                        Clients.All.SendAsync("ReceiveMessage", message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
