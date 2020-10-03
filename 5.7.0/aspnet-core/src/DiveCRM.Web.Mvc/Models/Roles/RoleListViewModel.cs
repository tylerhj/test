using System.Collections.Generic;
using DiveCRM.Roles.Dto;

namespace DiveCRM.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
