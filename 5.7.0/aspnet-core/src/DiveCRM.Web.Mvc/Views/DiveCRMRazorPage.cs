using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace DiveCRM.Web.Views
{
    public abstract class DiveCRMRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected DiveCRMRazorPage()
        {
            LocalizationSourceName = DiveCRMConsts.LocalizationSourceName;
        }
    }
}
