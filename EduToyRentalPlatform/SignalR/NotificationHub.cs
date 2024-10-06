using Microsoft.AspNetCore.SignalR;

namespace EduToyRentalPlatform.SignalR
{
	public class NotificationHub : Hub
	{
		public override async Task OnConnectedAsync()
		{
			await Clients.All.SendAsync("ReceiveNotification", $"{Context.ConnectionId} has connected to notification hub.");
		}

		public async Task PushNotificationToAll()
		{
			await Clients.All.SendAsync("ReceiveNotification", "Notification Sent To All");
		}
	}
}
