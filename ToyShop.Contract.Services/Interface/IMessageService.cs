using ToyShop.ModelViews.MessageModelViews;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToyShop.Contract.Services.Interface
{
    public interface IMessageService
    {
        Task<MessageViewModel> GetByIdAsync(string id); // Lấy tin nhắn theo ID
        Task<IEnumerable<MessageViewModel>> GetAllAsync(); // Lấy tất cả tin nhắn
        Task AddAsync(CreateMessageModel message); // Thêm tin nhắn mới
        Task UpdateAsync(UpdateMessageModel message); // Cập nhật tin nhắn
        Task DeleteAsync(string id); // Xóa tin nhắn
        Task<ResponseMessageModel> RespondToMessageAsync(ResponseMessageModel responseModel); // Phản hồi tin nhắn
    }
}
