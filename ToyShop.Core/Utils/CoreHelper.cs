using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.Core.Utils
{
    public class CoreHelper
    {
        public static DateTimeOffset SystemTimeNow => DateTimeParsing.ConvertToUtcPlus7(DateTimeOffset.Now);
    }
}
