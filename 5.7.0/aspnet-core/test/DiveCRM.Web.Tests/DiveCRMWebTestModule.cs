using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DiveCRM.EntityFrameworkCore;
using DiveCRM.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace DiveCRM.Web.Tests
{
    [DependsOn(
        typeof(DiveCRMWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class DiveCRMWebTestModule : AbpModule
    {
        public DiveCRMWebTestModule(DiveCRMEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(DiveCRMWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(DiveCRMWebMvcModule).Assembly);
        }
    }
}