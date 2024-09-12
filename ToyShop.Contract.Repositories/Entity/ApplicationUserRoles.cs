using Microsoft.AspNetCore.Identity;
using ToyShop.Core.Utils;

namespace ToyShop.Contract.Repositories.Entity
{
    public class ApplicationUserRoles : IdentityUserRole<Guid>
    {
        public DateTimeOffset CreatedTime { get; set; }

        public DateTimeOffset LastUpdatedTime { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }
        public ApplicationUserRoles()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }
    }
}
