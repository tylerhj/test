using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using DiveCRM.Controllers;

namespace DiveCRM.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : DiveCRMControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
