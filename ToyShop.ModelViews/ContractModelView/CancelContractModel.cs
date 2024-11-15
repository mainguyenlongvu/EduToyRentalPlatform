using ToyShop.Core.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ToyShop.Core.Utils;

using static ToyShop.Core.Base.BaseException;
using ToyShop.Core.Constants;
namespace ToyShop.ModelViews.ContractModelView
{
    public class CancelContractModel
    {

        [DisplayName("Trạng thái")]
        public string?   Status { get; set; }   // Status of the contract (string)
    }
}
