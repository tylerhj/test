using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DiveCRM.Authorization.Users;
using DiveCRM.Roles.Dto;
using DiveCRM.Users.Dto;

namespace DiveCRM.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<bool> ChangePassword(ChangePasswordDto input);

        IQueryable<User> FindAllByName(PagedUserResultRequestDto input);
    }
}
