using ToyShop.ModelViews.MessageModelViews;

namespace EduToyRentalPlatform.SignalR.Interfaces
{
    public interface IMessageHub
    {
        Task ReceiveMessage(string message);
        Task ReceiveMessage(CreateMessageModel message);
		Task ReceiveMessage(CreateMessageModel message, string receiverID);
	}
}