using Microsoft.AspNetCore.SignalR;
using ToyShop.Contract.Services;
using ToyShop.ModelViews.MessageModelViews;

namespace EduToyRentalPlatform.SignalR
{
	public class MessageHub : Hub
	{

		private readonly MessageService _messageService;

		public MessageHub(MessageService messageService)
		{
			_messageService = messageService;
		}

		public override async Task OnConnectedAsync()
		{
			await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has connected to Message hub.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">CreateMessageModel</param>
		/// <param name="receiverID">Receiver's ConnectionID</param>
		/// <returns></returns>
		public async Task SendMessage(CreateMessageModel message, string receiverID)
		{
			await Clients.Client(receiverID).SendAsync("ReceiveMessage", "Notification Sent To All");
		}
	}
}
