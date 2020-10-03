using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace DiveCRM.EntityFrameworkCore
{
    public static class DiveCRMDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<DiveCRMDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<DiveCRMDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
