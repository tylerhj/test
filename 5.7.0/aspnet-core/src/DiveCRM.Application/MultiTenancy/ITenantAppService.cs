using Abp.Application.Services;
using DiveCRM.MultiTenancy.Dto;

namespace DiveCRM.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

