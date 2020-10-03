using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DiveCRM.Configuration;

namespace DiveCRM.Web.Host.Startup
{
    [DependsOn(
       typeof(DiveCRMWebCoreModule))]
    public class DiveCRMWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public DiveCRMWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(DiveCRMWebHostModule).GetAssembly());
        }
    }
}
