using BAccurate.Models.User;
using SAM.Framework;
using SAM.Framework.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.AppServices
{
    public interface IUserAppService: ITransient
    {
        ResultListInfo<UserInfo> GetAllUsers();

        ResultInfo Save(UserParam param);

        ResultInfo Remove(int id);

        ResultDataInfo<UserDetailInfo> Get(int id);
    }
}
