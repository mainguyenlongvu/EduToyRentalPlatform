using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Base;

namespace ToyShop.Contract.Repositories.Entity
{
    [Table("FeedBack")]
    public class FeedBack : BaseEntity
    {
        //public User? User { get; set; }
        public string ToyId { get; set; } = null!;
        public virtual Toy? Toy { get; set; }
        public string? Content { get; set; }
    }
}
