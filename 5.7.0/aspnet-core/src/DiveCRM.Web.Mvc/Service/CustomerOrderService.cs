using Abp.Application.Services;
using Abp.EntityFrameworkCore;
using DiveCRM.Customers;
using DiveCRM.EntityFrameworkCore;
using DiveCRM.Web.DAL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;

namespace DiveCRM.Web.Service
{
    public class CustomerOrderService : BaseRepositoryAsync<CustomersOrder>, IApplicationService
    {
        private CustomerService cs;
        public CustomerOrderService(IDbContextProvider<DiveCRMDbContext> provider
            ,CustomerService cs) : base(provider)
        {
            this.cs = cs;
        }

        public PagedList<CustomersOrder> GetCustomerOrder(int pageIndex, string Keyword,DateTime? searchTime, int location,long? ParentID)
        {
            int count = 0;
            var q = FindPageList1(pageIndex, PageSizeWeb, out count
                , x => (ParentID == 0 || x.Location== location) 
                        &&(string.IsNullOrEmpty(Keyword) || x.CustomerName.Contains(Keyword))
                        && (searchTime==null || x.OrderTime.Date == searchTime)
                , true, y => y.OrderTime
                ); 
                return q;
        }

    }
}
