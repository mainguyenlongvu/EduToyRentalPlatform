using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Base;

namespace ToyShop.Contract.Repositories.Entity
{
    public partial class Transaction: BaseEntity
    {
        public int TranCode { get; set; }

        public DateTime? DateCreated { get; set; }

        public string Status { get; set; }

        public bool? Method { get; set; }

        public string? ContractId { get; set; }

        public virtual ContractEntity? ContractEntity { get; set; }
    }

}
