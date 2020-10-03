using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using DiveCRM.Authorization.Users;
using Microsoft.AspNetCore.Identity;

namespace DiveCRM.Controllers
{
    public abstract class DiveCRMControllerBase: AbpController
    {
        protected DiveCRMControllerBase()
        {
            LocalizationSourceName = DiveCRMConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

    }
}
