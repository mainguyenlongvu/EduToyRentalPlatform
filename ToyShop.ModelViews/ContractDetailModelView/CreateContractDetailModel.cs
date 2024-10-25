using ToyShop.Core.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ToyShop.Core.Utils;

using static ToyShop.Core.Base.BaseException;
using ToyShop.Core.Constants;
namespace ToyShop.ModelViews.ContractDetailModelView
{
    public class CreateContractDetailModel
    {
        [Required(ErrorMessage = "Phải nhập ToyId")]
        [DisplayName("Mã đồ chơi")]
        public string? ToyId { get; set; }   // Mã đồ chơi

        [Required(ErrorMessage = "Phải nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }   // Số lượng đồ chơi
        public DateOnly? DateStart { get; set; }
        public DateOnly? DateEnd { get; set; }
        public bool? ContractType { get; set; }


    }

}
