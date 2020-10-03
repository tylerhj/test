using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DiveCRM.Authorization;

namespace DiveCRM
{
    [DependsOn(
        typeof(DiveCRMCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class DiveCRMApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<DiveCRMAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(DiveCRMApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
