using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DiveCRM.Authorization;
using DiveCRM.Controllers;
using DiveCRM.Web.Service;
using Microsoft.AspNetCore.Http;
using DiveCRM.Customers;
using System;
using DiveCRM.Sessions;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace DiveCRM.Web.Controllers
{
    [AbpMvcAuthorize]
    public class CustomersController : DiveCRMControllerBase
    {
        protected CustomerService cs;
        protected DictService ds;
        protected ISessionAppService sas;
        public CustomersController(CustomerService cs
            ,DictService ds
            ,ISessionAppService sas)
        {
            this.cs = cs;
            this.ds = ds;
            this.sas = sas; 
        }

        public ActionResult Index(int pageIndex = 1, string Keyword = "")
        {
            var q = cs.GetCustomerByUserID(AbpSession.UserId??0,pageIndex, Keyword);
            return View(q);
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Customer customer = new Customer();
                customer.Name = collection["Name"];
                customer.Mobile = collection["Mobile"];
                customer.Age = int.Parse(collection["Age"]);
                customer.CreateTime = DateTime.Now;
                customer.Uid = AbpSession.UserId??0;
                customer.Location = sas.GetCurrentLoginInformations().Result.User.Location;
                cs.Create(customer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            var q = cs.Get(id);
            return View(q);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            try
            {
                cs.Edit(customer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            var q = cs.Get(id);
            return View(q);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                cs.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }

}
