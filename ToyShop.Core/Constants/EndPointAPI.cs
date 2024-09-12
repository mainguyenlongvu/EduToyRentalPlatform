using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.Core.Constants
{
    public static class EndPointAPI
    {
        public const string AreaName = "api";
        public static class Warehouse
        {
            private const string BaseEndpoint = "~/" + AreaName + "/warehouse-management";

            public const string Line = BaseEndpoint + "/lineproducts";
            public const string LineId = BaseEndpoint + "/lineproducts/{idlineproduct}";
            
            public const string Floor = BaseEndpoint + "/floorproducts";
            public const string FloorId = BaseEndpoint + "/floorproducts/{idFloorProduct}";

            public const string Chamber = BaseEndpoint + "/chambers";
            public const string ChamberId = BaseEndpoint + "/chambers/{idchamber}";

        }
    }
}
