using BAccurate.Models.Role;
using SAM.Framework;
using SAM.Framework.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.AppServices
{
    public interface IRoleAppService : ITransient
    {
        ResultListInfo<RoleInfo> GetAllRoles();

        ResultDataInfo<RoleDetailInfo> Get(int roleId);

        ResultInfo Save(RoleParam param);

        ResultInfo Remove(int id);
    }
}
