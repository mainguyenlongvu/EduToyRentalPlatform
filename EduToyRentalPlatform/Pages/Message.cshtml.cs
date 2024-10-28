using EduToyRentalPlatform.SignalR.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.MessageModelViews;

namespace ToyShop.Pages
{
    public class MessageModel : PageModel
    {
        private readonly IMessageService _messageService;
		private readonly IMessageHub _messageHub;


        public MessageModel(IMessageService messageService, IMessageHub messageHub)
        {
            _messageService = messageService;
			_messageHub = messageHub;
        }

        public List<MessageViewModel> Messages { get; set; } = new List<MessageViewModel>();

        public async Task OnGetAsync()
        {
            Messages = (List<MessageViewModel>)await _messageService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostSendMessageAsync(string messageInput)
        {
			

            if (!string.IsNullOrWhiteSpace(messageInput))
            {
                var newMessage = new CreateMessageModel
                {
                    ChatId = "1", // ID của chat, có thể thay đổi theo thực tế
                    SenderId = "user123", // ID người gửi, có thể thay đổi theo thực tế
                    MessageText = messageInput
                };

                try
                {
					await _messageHub.ReceiveMessage(newMessage, ClaimTypes.NameIdentifier);
					await _messageService.AddAsync(newMessage); // Gọi dịch vụ để thêm tin nhắn
					 
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi lưu tin nhắn: " + ex.Message);
                    return Page();
                }
            }

            return RedirectToPage(); // Quay lại trang hiện tại để làm mới danh sách tin nhắn
        }
    }
}
