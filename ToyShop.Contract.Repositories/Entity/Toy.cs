﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Base;

namespace ToyShop.Contract.Repositories.Entity
{
    [Table("Toy")]
    public class Toy:BaseEntity
    {
        public string? ToyName { get; set; }
        public string? ToyImg { get; set; }
        public string? ToyDescription { get; set; }
        public int ToyPrice { get; set; }
        public int ToyRemainingQuantity { get; set; }
        public int ToyQuantitySold { get; set; }
        public string? Option { get; set; }
        public virtual ICollection<FeedBack>? FeedBacks { get; set; }
        public virtual ICollection<ContractEntity>? ContractEntitys { get; set; }

    }
}
