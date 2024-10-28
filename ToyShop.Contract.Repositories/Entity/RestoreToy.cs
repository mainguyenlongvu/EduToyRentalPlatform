using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Base;

namespace ToyShop.Contract.Repositories.Entity
{
    public class RestoreToy : BaseEntity
    {
        public string? ContractId { get; set; }
        public double? TotalToyQuality { get; set; }
        public int? TotalReward { get; set; }
        public double? TotalMoney { get; set; }
        public string? Status { get; set; }

        public virtual ContractEntity? ContractEntity { get; set; }
        public virtual ICollection<RestoreToyDetail>? RestoreToyDetails { get; set; } = new List<RestoreToyDetail>();
    }
}
