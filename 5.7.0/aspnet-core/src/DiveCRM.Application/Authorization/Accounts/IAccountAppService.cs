using System.Threading.Tasks;
using Abp.Application.Services;
using DiveCRM.Authorization.Accounts.Dto;

namespace DiveCRM.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
