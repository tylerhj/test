

using Abp.Modules;
using DiveCRM.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DiveCRM.Web.DAL.Base
{
    public class BaseClass
    {
        public static int PageSizeAPI ;
        public static int PageSizeWeb ;
        IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        public BaseClass()
        {
            PageSizeAPI = int.Parse(config["PageSizeAPI"]);
            PageSizeWeb = int.Parse(config["PageSizeWeb"]);
        }


    }
}
