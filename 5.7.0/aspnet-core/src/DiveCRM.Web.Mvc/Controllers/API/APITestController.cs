using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.WebApi.Authorization;
using Abp.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiveCRM.Web.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class APITestController : AbpApiController,IApplicationService
    {

        public List<dynamic> Test()
        {
            List<dynamic> list = new List<dynamic>()
            {
                new {Name="t1",Value=1,Text="test1" },
                new {Name="t2",Value=2,Text="test2" },
                new {Name="t3",Value=3,Text="test3" },
            };
            return list;
        }
    }
}
