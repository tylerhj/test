using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using DiveCRM.Authorization.Roles;
using DiveCRM.Authorization.Users;
using DiveCRM.MultiTenancy;
using DiveCRM.Customers;
using DiveCRM.Dicts;

namespace DiveCRM.EntityFrameworkCore
{
    public class DiveCRMDbContext : AbpZeroDbContext<Tenant, Role, User, DiveCRMDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public DiveCRMDbContext(DbContextOptions<DiveCRMDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Dict> Dicts { get; set; }
        public DbSet<CustomersOrder> CustomersOrders { get; set; }
    }
}
