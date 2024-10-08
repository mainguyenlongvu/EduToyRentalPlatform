using ToyShop.ModelViews.MessageModelViews;

namespace EduToyRentalPlatform.SignalR.Interfaces
{
    public interface IMessageHub
    {
        Task OnConnectedAsync();
        Task SendMessage(CreateMessageModel message, string receiverID);
    }
}