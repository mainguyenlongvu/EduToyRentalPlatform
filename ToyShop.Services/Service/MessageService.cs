using ToyShop.ModelViews.MessageModelViews;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToyShop.Contract.Services.Interface;
using ToyShop.Repositories.Base;

namespace ToyShop.Contract.Services
{
    public class MessageService : IMessageService
    {
        private readonly ToyShopDBContext _context;

        public MessageService(ToyShopDBContext context)
        {
            _context = context;
        }

        public async Task<MessageViewModel> GetByIdAsync(string id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return null;

            return new MessageViewModel
            {
                Id = message.Id,
                ChatId = message.ChatId,
                SenderId = message.SenderId,
                MessageText = message.MessageText,
                CreatedTime = message.CreatedTime,
                LastUpdatedTime = message.LastUpdatedTime
            };
        }

        public async Task<IEnumerable<MessageViewModel>> GetAllAsync()
        {
            var messages = await _context.Messages.ToListAsync();
            return messages.Select(m => new MessageViewModel
            {
                Id = m.Id,
                ChatId = m.ChatId,
                SenderId = m.SenderId,
                MessageText = m.MessageText,
                CreatedTime = m.CreatedTime,
                LastUpdatedTime = m.LastUpdatedTime
            }).ToList();
        }


        public async Task AddAsync(CreateMessageModel message)
        {
            var newMessage = new Message
            {
                Id = Guid.NewGuid().ToString(), // Tạo ID mới cho tin nhắn
                ChatId = message.ChatId,
                SenderId = message.SenderId,
                MessageText = message.MessageText,
                CreatedTime = DateTimeOffset.UtcNow,
                LastUpdatedTime = DateTimeOffset.UtcNow
            };

            await _context.Messages.AddAsync(newMessage); // Thêm tin nhắn vào DbContext
            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
        }



        public async Task UpdateAsync(UpdateMessageModel message)
        {
            var existingMessage = await _context.Messages.FindAsync(message.Id);
            if (existingMessage != null)
            {
                existingMessage.MessageText = message.MessageText;
                existingMessage.LastUpdatedTime = DateTimeOffset.UtcNow;

                _context.Messages.Update(existingMessage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ResponseMessageModel> RespondToMessageAsync(ResponseMessageModel responseModel)
        {
            return await Task.FromResult(responseModel);
        }
    }
}
