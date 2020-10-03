using System.Collections.Generic;
using DiveCRM.Roles.Dto;

namespace DiveCRM.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}