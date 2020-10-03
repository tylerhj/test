using Abp.AspNetCore.Mvc.Authorization;
using DiveCRM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiveCRM.Web.Controllers
{
    [AbpMvcAuthorize]
    public class OrdersController : DiveCRMControllerBase
    {
    }
}
