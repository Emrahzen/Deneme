#nullable disable
using Microsoft.AspNetCore.SignalR.Client;

namespace denemeclass.SignalR
{
    public class RGateSignalRClient
    {
        private HubConnection _hubConnection;
        public RGateSignalRClient(string url)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string>("Connected", OnConnectedHub);

            _hubConnection.Closed += HubConnection_Closed;

        }

        protected void OnConnectedHub(string message)
        {
            Console.WriteLine("OnConnected " + message);
        }

        public async void Start()
        {
            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hub Connection Start Error = " + ex.Message);
            }
        }
        private Task HubConnection_Closed(Exception arg)
        {
            Task.Delay(new Random().Next(0, 5) * 1000);
            _hubConnection.StartAsync();

            return Task.CompletedTask;
        }
        public async void SendMessageService(string message)
        {
            try
            {
                await _hubConnection.InvokeAsync<string>("SendMessage", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SignalR APP Send Error" + ex.Message);
            }
        }
    }
}
