using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Base;
using ToyShop.Repositories.Entity;

namespace ToyShop.Contract.Repositories.Entity
{
    public class Message : BaseEntity
    {
        public string? ChatId { get; set; }
        public string? SenderId { get; set; }
        public string? MessageText { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual Chat? Chat { get; set; }

    }
}
