using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using DiveCRM.Controllers;
using DiveCRM.Web.Service;
using DiveCRM.Customers;
using System.Collections.Generic;
using System;
using System.Linq;
using DiveCRM.Authorization.Users;
using DiveCRM.Users;
using Newtonsoft.Json;
using DiveCRM.Web.Models.CustomerOrderReport;

namespace DiveCRM.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : DiveCRMControllerBase
    {
        private CustomerOrderService cos;
        private CustomerService cs;
        private IUserAppService us;
        public HomeController(CustomerOrderService cos,CustomerService cs, IUserAppService us)
        {
            this.cos = cos;
            this.cs = cs;
            this.us = us;
        }

        public ActionResult Index()
        {
            DateTime startDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime endDate = new DateTime(DateTime.Now.Year, 12, 31);
            List<CustomersOrder> list = cos.FindList(x => x.OrderTime >= startDate && x.OrderTime <= endDate).ToList() ?? new List<CustomersOrder>();
            List<Customer> customerList = cs.FindList(x => true).ToList();
            List<User> userList = us.FindAll().ToList();
            //当月销量
            ViewData["CurrentSales"] = list.Where(x => x.OrderTime.Month == DateTime.Now.Month).Count();
            //总销量
            ViewData["AllSales"] = list.Count;
            //客户总数
            ViewData["AllCustomer"] = customerList.Count;
            //销售总数
            ViewData["AllUser"] = userList.Count;
            //每月销量报表 
            ViewData["labels"] = JsonConvert.SerializeObject(list.Select(x => x.OrderTime.Month).Distinct());
            ViewData["data"] = JsonConvert.SerializeObject(list.GroupBy(x => x.OrderTime.Month).Select(x=>x.Count()).Distinct());
            //年度最佳客户
            var CurrentSalesBest = list.GroupBy(x => new { CustomerId = x.CustomerId, Name = x.CustomerName })
                .Select(x => new CutomerOrderReport
                {
                    Id=(long)x.Key.CustomerId,
                    Name = x.Key.Name,
                    Count = x.Count()
                })
                .OrderByDescending(x=>x.Count).ToList();
            //如果不足4条，补齐
            for (int i = CurrentSalesBest.Count; i < 4; i++)
            {
                CurrentSalesBest.Add(new CutomerOrderReport { Id = 0L, Name = "", Count = 0 }) ;
            }
            ViewBag.CurrentSalesBest = CurrentSalesBest;

            return View();
        }
    }
}
