using EduToyRentalPlatform.SignalR.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ToyShop.Contract.Services;
using ToyShop.ModelViews.MessageModelViews;

namespace EduToyRentalPlatform.SignalR
{
	//[Authorize]
    public class MessageHub : Hub<IMessageHub>
	{

		private readonly MessageService _messageService;

		public MessageHub(MessageService messageService)
		{
			_messageService = messageService;
		}

		public override async Task OnConnectedAsync()
		{
			await Clients.All.SendMessage(new CreateMessageModel(), $"{Context.ConnectionId} has connected to Message hub.");

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">CreateMessageModel</param>
		/// <param name="receiverID">Receiver's ConnectionID</param>
		/// <returns></returns>
		public async Task SendMessage(CreateMessageModel message, string receiverID)
		{
			await Clients.Client(receiverID).SendMessage(message, "Notification Sent To All");
		}


	}
}
