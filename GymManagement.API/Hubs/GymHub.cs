using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GymManagement.API.Hubs
{
    public class GymHub : Hub
    {
        // Hub methods can be called by clients to broadcast to others
        // E.g., a client can invoke NotifyUpdate, and the hub broadcasts it.
        public async Task NotifyTrainerAdded(object data) => await Clients.Others.SendAsync("TrainerAdded", data);
        public async Task NotifyTrainerUpdated(object data) => await Clients.Others.SendAsync("TrainerUpdated", data);
        public async Task NotifyTrainerDeleted(object data) => await Clients.Others.SendAsync("TrainerDeleted", data);
        
        public async Task NotifyPackageAdded(object data) => await Clients.Others.SendAsync("PackageAdded", data);
        public async Task NotifyPackageUpdated(object data) => await Clients.Others.SendAsync("PackageUpdated", data);
        public async Task NotifyPackageDeleted(object data) => await Clients.Others.SendAsync("PackageDeleted", data);

        public async Task NotifySubscriptionAdded(object data) => await Clients.Others.SendAsync("SubscriptionAdded", data);
        public async Task NotifySubscriptionUpdated(object data) => await Clients.Others.SendAsync("SubscriptionUpdated", data);
        public async Task NotifySubscriptionDeleted(object data) => await Clients.Others.SendAsync("SubscriptionDeleted", data);
    }
}
