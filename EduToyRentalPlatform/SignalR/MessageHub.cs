using EduToyRentalPlatform.SignalR.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ToyShop.Contract.Repositories.Entity;
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
			await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.GetUserId());

			await Clients.Client(Context.ConnectionId).ReceiveMessage($"{Context.ConnectionId} has connected.");
			await base.OnConnectedAsync();
			

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">CreateMessageModel</param>
		/// <param name="receiverID">Receiver's ConnectionID</param>
		/// <returns></returns>
		public async Task SendMessage(CreateMessageModel message, string receiverID)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{message.SenderId}");
			await Clients.Client(receiverID).ReceiveMessage(message);
		}


	}
}
