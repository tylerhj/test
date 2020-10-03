using Abp.Authorization;
using DiveCRM.Authorization.Roles;
using DiveCRM.Authorization.Users;

namespace DiveCRM.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
