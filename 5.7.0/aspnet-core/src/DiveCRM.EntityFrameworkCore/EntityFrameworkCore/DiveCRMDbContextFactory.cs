using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using DiveCRM.Configuration;
using DiveCRM.Web;

namespace DiveCRM.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class DiveCRMDbContextFactory : IDesignTimeDbContextFactory<DiveCRMDbContext>
    {
        public DiveCRMDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DiveCRMDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DiveCRMDbContextConfigurer.Configure(builder, configuration.GetConnectionString(DiveCRMConsts.ConnectionStringName));

            return new DiveCRMDbContext(builder.Options);
        }
    }
}
