using BAccurate.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAM.Framework;

namespace BAccurate.Domain
{
    public interface IRoleAndResDepend:ITransient
    {
        List<ResourceInfo> GetAllResourceInfos();
        List<RoleAndResInfo> GetAllRoleAndRes();
    }
}
