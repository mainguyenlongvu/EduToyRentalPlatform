﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Base;
using ToyShop.Repositories.Entity;

namespace ToyShop.Contract.Repositories.Entity
{
    public partial class ContractEntity : BaseEntity
    {
        public Guid? UserId { get; set; }
        public string? ToyId { get; set; }
        public string? RestoreToyId { get; set; }
        public string? StaffConfirmed { get; set; }
        public double? TotalValue { get; set; }
        public int? NumberOfRentals { get; set; }
        public DateOnly? DateCreated { get; set; }
        public bool? ContractType { get; set; }
        public DateOnly? DateStart { get; set; }
        public DateOnly? DateEnd { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Delivery>? Deliveries { get; set; } = new List<Delivery>();
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual RestoreToy? RestoreToy { get; set; }
        public virtual Toy? Toy { get; set; }

    }

}
