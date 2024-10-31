using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.RestoreToyModelViews
{
    public class UpdateRestoreModel
    {
        public double? TotalToyQuality { get; set; }
        public int? TotalReward { get; set; }
        public double? TotalMoney { get; set; }
        public string? Status { get; set; }
    }
}
