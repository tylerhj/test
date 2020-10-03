using System.Collections.Generic;
using DiveCRM.Dicts;
using DiveCRM.Roles.Dto;

namespace DiveCRM.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
        public List<Dict> Dicts { get; set; }
    }
}
