using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Configuration;
using LTMCompanyName.YoyoCmsTemplate.Helpers;

namespace LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class YoyoCmsTemplateDbContextFactory : IDesignTimeDbContextFactory<YoyoCmsTemplateDbContext>
    {
        public YoyoCmsTemplateDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<YoyoCmsTemplateDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            YoyoCmsTemplateDbContextConfigurer.Configure(builder,
                configuration.GetConnectionString(YoyoCmsTemplateConsts.ConnectionStringName));

            return new YoyoCmsTemplateDbContext(builder.Options);
        }
    }
}