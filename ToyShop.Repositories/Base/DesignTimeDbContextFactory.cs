using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.Repositories.Base
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ToyShopDBContext>
    {
        public ToyShopDBContext CreateDbContext(string[] args)
        {

            var builder = new DbContextOptionsBuilder<ToyShopDBContext>();

            builder.UseSqlServer("server =localhost;database=ToyShop;uid=sa;pwd=sa;Trusted_Connection=True;Trust Server Certificate=True;Timeout=30;");

            return new ToyShopDBContext(builder.Options);
        }
    }
}
