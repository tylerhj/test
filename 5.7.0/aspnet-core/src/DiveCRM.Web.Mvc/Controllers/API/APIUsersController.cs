using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.WebApi.Controllers;
using DiveCRM.Authorization.Users;
using DiveCRM.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiveCRM.Web.Controllers.API
{
    [Route("api/Users/[action]")]
    [ApiController]
    public class APIUsersController : AbpApiController, IApplicationService
    {
        UserManager um;
        IUserAppService us;
        public APIUsersController(IUserAppService us, UserManager um)
        {
            this.us = us;
            this.um = um;
        }

        [HttpPost]
        public dynamic GetUsersByName(string AjaxQuery)
        {
            var q = us.FindAllByName(new Users.Dto.PagedUserResultRequestDto() { Keyword = AjaxQuery })
                .Select(x => new
                {
                    value = x.Id,
                    text = x.UserName,
                }).ToList();
            return q;
        }
    }
}
