using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToyShop.Contract.Repositories.Base;

namespace ToyShop.Contract.Repositories.Entity
{
    [Table("Toy")]
    public class Toy : BaseEntity
    {
        [Required(ErrorMessage = "Toy name is required.")]
        [StringLength(100, ErrorMessage = "Toy name cannot exceed 100 characters.")]
        public string? ToyName { get; set; }

        public string? ToyImg { get; set; }

        [Required(ErrorMessage = "Toy description is required.")]
        [StringLength(500, ErrorMessage = "Toy description cannot exceed 500 characters.")]
        public string? ToyDescription { get; set; }

        [Required(ErrorMessage = "Rent price is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Rent price must be a non-negative value.")]
        public int ToyPriceRent { get; set; }

        [Required(ErrorMessage = "Sale price is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Sale price must be a non-negative value.")]
        public int ToyPriceSale { get; set; }

        [Required(ErrorMessage = "Remaining quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Remaining quantity cannot be negative.")]
        public int ToyRemainingQuantity { get; set; }

        [Required(ErrorMessage = "Quantity sold is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity sold cannot be negative.")]
        public int ToyQuantitySold { get; set; }

        [StringLength(200, ErrorMessage = "Option cannot exceed 200 characters.")]
        public string? Option { get; set; }

        public virtual ICollection<FeedBack>? FeedBacks { get; set; }
        public virtual ICollection<ContractDetail>? ContractDetails { get; set; }
    }
}
