using System.Collections.Generic;
using System.Linq;
using DiveCRM.Dicts;
using DiveCRM.Roles.Dto;
using DiveCRM.Users.Dto;

namespace DiveCRM.Web.Models.Users
{
    public class EditUserModalViewModel
    {
        public UserDto User { get; set; }
        public List<Dict> Dicts { get; set; }
        public IReadOnlyList<RoleDto> Roles { get; set; }

        public bool UserIsInRole(RoleDto role)
        {
            return User.RoleNames != null && User.RoleNames.Any(r => r == role.NormalizedName);
        }
    }
}
