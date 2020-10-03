using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.WebApi.Controllers;
using DiveCRM.Authorization.Users;
using DiveCRM.Customers;
using DiveCRM.Web.Models.Common.Modals;
using DiveCRM.Web.Service;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiveCRM.Web.Controllers.API
{
    [Route("api/Customers/[action]")]
    [ApiController]
    public class APICustomersController : AbpApiController, IApplicationService
    {
        CustomerService cs;
        UserManager um;
        public APICustomersController(CustomerService cs, UserManager um)
        {
            this.cs = cs;
            this.um = um;
        }


        [HttpPost]
        public dynamic GetCustomerByName(string AjaxQuery)
        {
            var q = cs.GetCustomerByName(1, AjaxQuery)
                .Select(x => new {
                    value = x.Id,
                    text = x.Name,
                    responsiblePersonId = x.Uid,
                    responsiblePersonName = um.GetUserById(x.Uid).UserName
                }).ToList(); 
            return q;
        }
    }
}
