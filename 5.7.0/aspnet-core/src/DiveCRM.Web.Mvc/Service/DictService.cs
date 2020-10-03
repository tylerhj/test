using Abp.Application.Services;
using Abp.EntityFrameworkCore;
using DiveCRM.Authorization.Users;
using DiveCRM.Dicts;
using DiveCRM.EntityFrameworkCore;
using DiveCRM.Web.DAL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiveCRM.Web.Service
{
    public class DictService : BaseRepositoryAsync<Dict>, IApplicationService
    {
        private UserManager um;
        public DictService(IDbContextProvider<DiveCRMDbContext> provider
            , UserManager um) 
            : base(provider)
        {
            this.um = um;
        }

        public List<Dict> GetPoolTypeAll()
        {
            List<Dict> PoolType = new List<Dict>();
            var PoolTypeSH = FindList(x => x.ParentId == (int)EnumList.PoolTypeSH).ToList();
            var PoolTypeHZ = FindList(x => x.ParentId == (int)EnumList.PoolTypeHZ).ToList();
            PoolType.AddRange(PoolTypeSH);
            PoolType.AddRange(PoolTypeHZ);
            return PoolType;
        }

        public List<Dict> GetPoolType(User curUser)
        {
            List<Dict> PoolType=new List<Dict>();
            if (curUser.ParentID == 0)
            {
                var PoolTypeSH = FindList(x => x.ParentId == (int)EnumList.PoolTypeSH).ToList();
                var PoolTypeHZ = FindList(x => x.ParentId == (int)EnumList.PoolTypeHZ).ToList();
                PoolType.AddRange(PoolTypeSH);
                PoolType.AddRange(PoolTypeHZ);
            }
            else if (curUser.Location == 2)
                PoolType = FindList(x => x.ParentId == (int)EnumList.PoolTypeSH).ToList();
            else if (curUser.Location == 3)
                PoolType = FindList(x => x.ParentId == (int)EnumList.PoolTypeHZ).ToList();
            else
                PoolType = null;
            return PoolType;
        }
    }
}
