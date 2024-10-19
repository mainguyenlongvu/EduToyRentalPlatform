using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Core.Constants;
using ToyShop.Core.Store;
using static ToyShop.Core.Base.BaseException;

namespace ToyShop.ModelViews.ContractDetailModelView
{
    public class UpdateContractDetailModel
    {

        [Required(ErrorMessage = "Phải nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }   // Số lượng đồ chơi

    }


}
