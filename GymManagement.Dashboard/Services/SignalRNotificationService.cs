using Microsoft.AspNetCore.SignalR.Client;

namespace GymManagement.Dashboard.Services
{
    public class SignalRNotificationService : IAsyncDisposable
    {
        private readonly HubConnection _hubConnection;

        public SignalRNotificationService(IConfiguration configuration)
        {
            var hubUrl = configuration["ApiHubUrl"] ?? "http://localhost:5191/gymHub";
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();

            // Start connection in background
            _ = StartAsync();
        }

        private async Task StartAsync()
        {
            try
            {
                await _hubConnection.StartAsync();
            }
            catch
            {
                // Optionally retry or log
            }
        }

        public async Task NotifyTrainerAdded(object data) => await TryInvokeAsync("NotifyTrainerAdded", data);
        public async Task NotifyTrainerUpdated(object data) => await TryInvokeAsync("NotifyTrainerUpdated", data);
        public async Task NotifyTrainerDeleted(object data) => await TryInvokeAsync("NotifyTrainerDeleted", data);
        
        public async Task NotifyPackageAdded(object data) => await TryInvokeAsync("NotifyPackageAdded", data);
        public async Task NotifyPackageUpdated(object data) => await TryInvokeAsync("NotifyPackageUpdated", data);
        public async Task NotifyPackageDeleted(object data) => await TryInvokeAsync("NotifyPackageDeleted", data);
        
        public async Task NotifySubscriptionAdded(object data) => await TryInvokeAsync("NotifySubscriptionAdded", data);
        public async Task NotifySubscriptionUpdated(object data) => await TryInvokeAsync("NotifySubscriptionUpdated", data);
        public async Task NotifySubscriptionDeleted(object data) => await TryInvokeAsync("NotifySubscriptionDeleted", data);

        private async Task TryInvokeAsync(string methodName, object arg)
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
                await StartAsync();
                
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync(methodName, arg);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
