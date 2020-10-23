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
            ViewData["CurrentSales"] = list.Where(x => x.OrderTime.Month == DateTime.Now.Month).Count();
            ViewData["AllSales"] = list.Count;
            ViewData["AllCustomer"] = customerList.Count;
            ViewData["AllUser"] = userList.Count;

            ViewData["labels"] = JsonConvert.SerializeObject(list.Select(x => x.OrderTime.Month).Distinct());
            ViewData["data"] = JsonConvert.SerializeObject(list.GroupBy(x => x.OrderTime.Month).Select(x=>x.Count()).Distinct());
            return View();
        }
    }
}
