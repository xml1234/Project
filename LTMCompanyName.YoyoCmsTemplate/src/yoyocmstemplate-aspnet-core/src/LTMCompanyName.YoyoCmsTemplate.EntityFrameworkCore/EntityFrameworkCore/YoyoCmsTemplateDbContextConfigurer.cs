using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore
{
    public static class YoyoCmsTemplateDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<YoyoCmsTemplateDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString, p => p.UseRowNumberForPaging());
            //builder.UseLazyLoadingProxies().UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<YoyoCmsTemplateDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
            //builder.UseLazyLoadingProxies().UseSqlServer(connection);
        }
    }
}