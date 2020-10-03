using Abp.Application.Services;
using Abp.EntityFrameworkCore;
using DiveCRM.Customers;
using DiveCRM.EntityFrameworkCore;
using DiveCRM.Web.DAL.Base;
using NUglify.Helpers;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;

namespace DiveCRM.Web.Service
{
    public class CustomerService: BaseRepositoryAsync<Customer>, IApplicationService
    {
        public CustomerService(IDbContextProvider<DiveCRMDbContext> provider):base(provider){}

        public Customer Get(long customerId)
        {
            var q= _provider.GetDbContext().Customers.First(x => x.Id == customerId);
            return q;
        }

        public PagedList<Customer> GetCustomerByUserID(long userID,int pageIndex,string keyword)
        {
            userIds.Add(userID);
            GetUsersIds(userID);
            int count = 0;
            var q = FindPageList1(pageIndex, PageSizeWeb, out count
                , x => (string.IsNullOrEmpty(keyword) || x.Name.Contains(keyword)) && userIds.Contains(x.Uid)
                , true, y => y.Id
                );
            return q;
        }

        /// <summary>
        /// Test service
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<Customer> GetCustomerByName(int pageIndex , string keyword)
        {
            int count = 0;
            var q = FindPageList(pageIndex, PageSizeWeb, out count
                , x => (string.IsNullOrEmpty(keyword) || x.Name.Contains(keyword))
                , true, y => y.Name).ToList();
            return q;
        }

        public List<long> GetUserBranch(long userId)
        {
            GetUsersIds(userId);
            return userIds;
        }

        private List<long> userIds = new List<long>();
        private void GetUsersIds(long userID)
        {
            var q = _provider.GetDbContext().Users.Where(x => x.ParentID == userID).ToList();
            for (int i = 0; i < q?.Count; i++)
            {
                GetUsersIds(q[i].Id);
            };
            userIds.Add(userID);
        }

        public Customer Create(Customer customer)
        {
            _provider.GetDbContext().Customers.Add(customer);
            _provider.GetDbContext().SaveChanges();
            return customer;
        }

        public Customer Edit(Customer customer)
        {
            var q = _provider.GetDbContext().Customers.FirstOrDefault(x=>x.Id==customer.Id);
            q.Name = customer.Name;
            q.Mobile = customer.Mobile;
            q.Age = customer.Age;
            q.IsActive = customer.IsActive;
            _provider.GetDbContext().SaveChanges();
            return customer;
        }

        public int Delete(long id)
        {
            var q = _provider.GetDbContext().Customers.FirstOrDefault(x => x.Id == id);
            _provider.GetDbContext().Customers.Remove(q);
            var v = _provider.GetDbContext().SaveChanges();

            return v;
        }
    }
}
