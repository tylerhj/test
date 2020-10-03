using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiveCRM.Test
{
    public class TestService :  IApplicationService
    {
        
        public List<dynamic> Test()
        {
            List<dynamic> list = new List<dynamic>()
            {
                new {Name="t11",Value=3,Text="11" },
                new {Name="t22",Value=1,Text="22" },
                new {Name="t33",Value=2,Text="33" },
            };
            return list;
        }
    }
}
