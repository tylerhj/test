using System.Threading.Tasks;
using Abp.Application.Services;
using DiveCRM.Sessions.Dto;

namespace DiveCRM.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
