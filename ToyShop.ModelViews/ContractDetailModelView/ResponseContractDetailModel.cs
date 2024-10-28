using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.ContractDetailModelView
{
    public class ResponseContractDetailModel
    {
        [DisplayName("Mã hợp đồng")]
        public string ContractId { get; set; }   // Mã hợp đồng
        public string Id { get; set; }
        [DisplayName("Mã đồ chơi")]
        public string? ToyId { get; set; }   // Mã đồ chơi
        public string ToyName { get; set; }
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }   // Số lượng đồ chơi
        [DisplayName("Giá")]
        public decimal Price { get; set; }   // Giá của đồ chơi
        public bool? ContractType { get; set; }
        public DateOnly? DateStart { get; set; }
        public DateOnly? DateEnd { get; set; }
    }
}
