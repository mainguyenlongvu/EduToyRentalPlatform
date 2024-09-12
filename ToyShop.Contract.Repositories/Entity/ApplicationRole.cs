using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using ToyShop.Core.Utils;

namespace ToyShop.Contract.Repositories.Entity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? FullName { get; set; }
        public DateTimeOffset CreatedTime { get; set; }

        public DateTimeOffset LastUpdatedTime { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }
        public ApplicationRole()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }
        
    }
}