using Microsoft.AspNetCore.Identity;
using ToyShop.Core.Utils;

namespace ToyShop.Repositories.Entity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        
        public string FullName { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        // Navigation properties
        public ApplicationUser()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }
    }
}
