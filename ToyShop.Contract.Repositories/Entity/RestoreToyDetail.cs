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
    public class RestoreToyDetail : BaseEntity
    {
        public string? RestoreToyId { get; set; }
        public double? ToyQuality { get; set; }
        public int? Reward { get; set; }
        public string? ToyName { get; set; }
        public bool? IsReturn { get; set; }
        public double? OverdueTime { get; set; }
        public double? TotalMoney { get; set; }

        public virtual RestoreToy? RestoreToy { get; set; }
    }
}
