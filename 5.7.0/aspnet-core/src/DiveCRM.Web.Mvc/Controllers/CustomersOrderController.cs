using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using DiveCRM.Authorization.Users;
using DiveCRM.Controllers;
using DiveCRM.Customers;
using DiveCRM.Dicts;
using DiveCRM.Web.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiveCRM.Web.Controllers
{
    public class CustomersOrderController : DiveCRMControllerBase
    {
        private UserManager um;
        private CustomerService cs;
        private CustomerOrderService cos;
        private DictService ds;
        public CustomersOrderController(UserManager um, CustomerService cs , CustomerOrderService cos, DictService ds) {
            this.um = um;
            this.cs = cs;
            this.cos = cos;
            this.ds = ds;
        }

        public IActionResult Index(int pageIndex = 1, string Keyword = "",DateTime? searchTime=null)
        {
            var State = ds.FindList(x => x.ParentId == (int)EnumList.State).ToList();
            ViewData["State"] =State;
            ViewData["OrderTime"] = searchTime==null?"": ((DateTime)searchTime).ToString("yyyy-MM-dd");
            ViewData["PoolType"] = ds.GetPoolTypeAll();
            ViewData["CourseType"] = ds.FindList(x => x.ParentId == (int)EnumList.CourseType).ToList();
            User curUser = um.FindByIdAsync(AbpSession.UserId.ToString()).Result;
            var q = cos.GetCustomerOrder(pageIndex, Keyword,searchTime, curUser.Location,curUser.ParentID);
            q.ForEach(x =>
            {
                x.Customer = cs.Find(y => y.Id == x.CustomerId);
                x.Coach= um.GetUserById(x.CoachId ?? 1);
                x.ResponsiblePerson= um.GetUserById(x.ResponsiblePersonId??1);
            });

            return View(q);
        }

        // GET: CustomerController/Create
        [UnitOfWork]
        public ActionResult Create()
        {

            User curUser = um.FindByIdAsync(AbpSession.UserId.ToString()).Result;
            List<Dict> PoolType = ds.GetPoolType(curUser);

            var CoachType = ds.FindList(x => x.ParentId == (int)EnumList.CoachType).ToList();
            var CourseType = ds.FindList(x => x.ParentId == (int)EnumList.CourseType).ToList();
            var State = ds.FindList(x => x.ParentId == (int)EnumList.State).ToList();
            var Customers = cs.FindList(x => true).ToList();
            ViewData["PoolType"]= new SelectList(PoolType, "Id", "DValue");
            ViewData["CoachType"] = new SelectList(CoachType, "Id", "DValue");
            ViewData["CourseType"] = new SelectList(CourseType, "Id", "DValue");
            ViewData["State"] = new SelectList(State, "Id", "DValue");
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        public ActionResult Create(CustomersOrder entity)
        {
            try
            {
                //用户不存在，则添加
                if (entity.CustomerId == null || entity.CustomerId == 0)
                {
                    Customer c = new Customer();
                    c.Name = entity.CustomerName??"";
                    c.Mobile = entity.Mobile;
                    c.Uid = (long)AbpSession.UserId;
                    c.CreateTime = DateTime.Now;
                    cs.Add(c);

                    entity.CustomerId = c.Id;
                }

                User curUser = um.FindByIdAsync(AbpSession.UserId.ToString()).Result;
                CustomersOrder order = new CustomersOrder();
                order.CustomerId = entity.CustomerId;
                order.CustomerName = entity.CustomerName;
                order.OrderTime = entity.OrderTime;
                order.PoolType = entity.PoolType;
                order.CourseType = entity.CourseType;
                order.CoachType = entity.CoachType;
                order.CoachId = entity.CoachId;
                order.ResponsiblePersonId = entity.ResponsiblePersonId??AbpSession.UserId;
                order.State = entity.State;
                order.Location = curUser.Location;
                order.CreateTime = DateTime.Now;
                order.Remark = entity.Remark;

                cos.Add(order);
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
            var q = cos.Find(x=>x.Id==id);
            q.Customer = cs.Find(x => x.Id == q.CustomerId);
            q.Coach=um.GetUserById(q.CoachId ?? 1);
            q.ResponsiblePerson = um.GetUserById(q.ResponsiblePersonId??1);
            User curUser = um.FindByIdAsync(AbpSession.UserId.ToString()).Result;
            List<Dict> PoolType = ds.GetPoolType(curUser);

            var CoachType = ds.FindList(x => x.ParentId == (int)EnumList.CoachType).ToList();
            var CourseType = ds.FindList(x => x.ParentId == (int)EnumList.CourseType).ToList();
            var Customers = cs.FindList(x => true).ToList();
            var State=ds.FindList(x => x.ParentId == (int)EnumList.State).ToList();
            ViewData["PoolType"] = new SelectList(PoolType, "Id", "DValue",q.PoolType);
            ViewData["CoachType"] = new SelectList(CoachType, "Id", "DValue",q.CoachType);
            ViewData["CourseType"] = new SelectList(CourseType, "Id", "DValue",q.CourseType);
            ViewData["State"] = new SelectList(State, "Id", "DValue", q.State);
            
            return View(q);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        public ActionResult Edit(CustomersOrder entity)
        {
            entity.CustomerName = entity.CustomerName_Display;
            try
            {
                //用户不存在，则添加
                if (entity.CustomerId == null || entity.CustomerId == 0)
                {
                    Customer c = new Customer();
                    c.Name = entity.CustomerName ?? "";
                    c.Mobile = entity.Mobile;
                    c.Uid = (long)AbpSession.UserId;
                    c.CreateTime = DateTime.Now;
                    cs.Add(c);

                    entity.CustomerId = c.Id;
                    entity.ResponsiblePersonId = AbpSession.UserId;
                }
                else
                {
                    Customer c = cs.Find(x=>x.Id==entity.CustomerId);
                    c.Name = entity.CustomerName ?? "";
                    c.Mobile = entity.Mobile;
                    cs.Update(c);

                }
                User curUser = um.FindByIdAsync(AbpSession.UserId.ToString()).Result;
                var q = cos.Find(x => x.Id == entity.Id);
                q.CustomerId = entity.CustomerId;
                q.CustomerName = entity.CustomerName;
                q.OrderTime = entity.OrderTime;
                q.PoolType = entity.PoolType;
                q.CourseType = entity.CourseType;
                q.CoachType = entity.CoachType;
                q.CoachId = entity.CoachId;
                q.ResponsiblePersonId = entity.ResponsiblePersonId ?? AbpSession.UserId;
                q.State = entity.State;
                q.Location = curUser.Location;
                q.Remark = entity.Remark;
                cos.Update(q);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult UpdateState(CustomersOrder entity, DateTime? searchTime = null)
        {
            ViewData["OrderTime"] = searchTime == null ? "" : ((DateTime)searchTime).ToString("yyyy-MM-dd");
            var q = cos.Find(x => x.Id == entity.Id);
            q.State = entity.State;
            cos.Update(q);
            return RedirectToAction(nameof(Index));
        }

    }
}
