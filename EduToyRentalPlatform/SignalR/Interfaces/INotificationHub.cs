using System.Runtime.CompilerServices;

namespace EduToyRentalPlatform.SignalR.Interfaces
{
	public interface INotificationHub
	{
		Task NotifyAll(string message);
		Task Notify(string message);
	}
}
