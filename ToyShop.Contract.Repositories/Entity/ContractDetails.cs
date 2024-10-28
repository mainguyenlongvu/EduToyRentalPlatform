using ToyShop.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.Contract.Repositories.Entity
{
    public class ContractDetail : BaseEntity
    {
        public string ContractId { get; set; }
        public virtual ContractEntity? Contract { get; set; }
        public bool? ContractType { get; set; }
        public string? ToyId { get; set; }
        public DateOnly? DateStart { get; set; }
        public DateOnly? DateEnd { get; set; }
        public virtual Toy? Toy { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
