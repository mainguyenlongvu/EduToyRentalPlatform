using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Base;

namespace ToyShop.Contract.Repositories.Entity
{
    public partial class Delivery : BaseEntity
    {
        public DateOnly? DateSend { get; set; }

        public DateOnly? DateRecieved { get; set; }

        public double? DeliveryCost { get; set; }

        public string? ContractId { get; set; }

        public virtual ContractEntity? ContractEntity { get; set; }
    }
}
