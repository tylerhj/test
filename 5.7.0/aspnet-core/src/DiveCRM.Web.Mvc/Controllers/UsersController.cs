using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DiveCRM.Authorization;
using DiveCRM.Controllers;
using DiveCRM.Users;
using DiveCRM.Web.Models.Users;
using DiveCRM.Web.Service;
using DiveCRM.Dicts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace DiveCRM.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Users)]
    public class UsersController : DiveCRMControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly DictService ds;
        public UsersController(IUserAppService userAppService, DictService ds)
        {
            _userAppService = userAppService;
            this.ds = ds;
        }

        public async Task<ActionResult> Index()
        {
            var roles = (await _userAppService.GetRoles()).Items;
            var dicts = await ds.FindListAsync(x => x.ParentId == (int)EnumList.Location);
            var model = new UserListViewModel
            {
                Roles = roles,
                Dicts = dicts
            };
            return View(model);
        }

        public async Task<ActionResult> EditModal(long userId)
        {
            var user = await _userAppService.GetAsync(new EntityDto<long>(userId));
            var dict = await ds.FindListAsync(x => x.ParentId == (int)EnumList.Location);
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new EditUserModalViewModel
            {
                User = user,
                Roles = roles,
                Dicts = dict
            };
            return PartialView("_EditModal", model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}
