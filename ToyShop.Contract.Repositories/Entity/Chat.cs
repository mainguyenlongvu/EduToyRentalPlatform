using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Base;
using ToyShop.Repositories.Entity;

namespace ToyShop.Contract.Repositories.Entity
{
    public class Chat : BaseEntity
    {
        public string? PartnerId { get; set; }
        public Guid? UserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }

    }
}

