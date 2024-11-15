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
		private readonly ConnectionStorage<string> _storage;

		public MessageHub(MessageService messageService, ConnectionStorage<string> storage)
		{
			_messageService = messageService;
			_storage = storage;
		}

		public override async Task OnConnectedAsync()
		{
			await base.OnConnectedAsync();
			_storage.Add(Context.User.Identity.GetUserId(), Context.ConnectionId);

			string userId = Context.User.Identity.GetUserId();

			if (!_storage.GetConnections(userId).Contains(Context.ConnectionId))
			{
				_storage.Add(userId, Context.ConnectionId);
			}
			await Clients.Client(Context.ConnectionId).ReceiveMessage($"{Context.ConnectionId} has connected.");
		}

		public override async Task OnDisconnectedAsync(Exception ex)
		{
			await base.OnDisconnectedAsync(ex);
			_storage.Remove(Context.User.Identity.GetUserId(), Context.ConnectionId);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">CreateMessageModel</param>
		/// <param name="receiverID">Receiver's UserId</param>
		/// <returns></returns>
		public async Task SendMessage(CreateMessageModel message, string receiverUserId)
		{
			string connectionId = _storage.GetConnections(receiverUserId).FirstOrDefault();
			if (connectionId != null)
			{
				await Clients.Client(connectionId).ReceiveMessage(message);
			}
			await _messageService.AddAsync(message);
		}
	}
}
