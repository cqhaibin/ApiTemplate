using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Models.Auth;
using SAM.Framework;

namespace BAccurate.Domain
{
    /// <summary>
    /// 认证相关依赖
    /// </summary>
    public interface IReadAuthRepository:ITransient
    {
        UserInfo GetUserInfo(int userId);
    }
}
