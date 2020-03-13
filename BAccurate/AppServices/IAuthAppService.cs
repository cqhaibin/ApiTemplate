using BAccurate.Models.Auth;
using SAM.Framework;
using SAM.Framework.Result;
using BAccurate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.AppServices
{
    /// <summary>
    /// 认证登录相关接口
    /// </summary>
    public interface IAuthAppService: ITransient
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        ResultDataInfo<LoginResultInfo> Login(LoginInfo info, RequestClientInfo clientInfo, string token = "");

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        ResultInfo Logout(string token);
    }
}
