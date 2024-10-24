using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Core.Utils;

namespace ToyShop.Repositories.Entity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        
        public string FullName { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string? Phone { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public int Money { get; set; }
        public virtual ICollection<ContractEntity>? ContractEntitys { get; set; }
        public virtual ICollection<Chat>? Chats { get; set; }
        public virtual ICollection<FeedBack>? FeedBacks { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        // Navigation properties
        public ApplicationUser()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }
    }
}
