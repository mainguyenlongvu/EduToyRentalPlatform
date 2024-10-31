using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.RestoreToyDetailModelViews
{
    public class UpdateRestoreDetailModel
    {
        public int? ToyQuality { get; set; }
        public int? Reward { get; set; }
        public string? ToyId { get; set; }
        public string? ToyName { get; set; }
        public bool? IsReturn { get; set; }
        public double? OverdueTime { get; set; }
        public int? TotalMoney { get; set; }
        public int? Compensation { get; set; }
    }
}
