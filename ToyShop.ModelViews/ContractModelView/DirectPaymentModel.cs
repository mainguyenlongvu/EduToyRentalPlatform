using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.ContractModelView
{
    public class DirectPaymentModel
    {
        [DataType(DataType.Text)]
        [DisplayName("Ghi chú")]
        public string? Note { get; set; }

    }
}
