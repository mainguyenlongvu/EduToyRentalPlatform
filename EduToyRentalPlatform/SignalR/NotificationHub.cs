using EduToyRentalPlatform.SignalR.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace EduToyRentalPlatform.SignalR
{
	public class NotificationHub : Hub<INotificationHub>
	{
		public override async Task OnConnectedAsync()
		{
			await Clients.Client(Context.ConnectionId).Notify($"{Context.ConnectionId} has connected to notification hub.");
		}

		public async Task PushNotificationToAll(string message)
		{
			await Clients.All.NotifyAll(message);
		}

		
	}
}
