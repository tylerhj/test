using Abp.AspNetCore.Mvc.ViewComponents;

namespace DiveCRM.Web.Views
{
    public abstract class DiveCRMViewComponent : AbpViewComponent
    {
        protected DiveCRMViewComponent()
        {
            LocalizationSourceName = DiveCRMConsts.LocalizationSourceName;
        }
    }
}
