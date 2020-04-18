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

        ResultListInfo<MenuInfo> GetMenus(string token);

        /// <summary>
        /// 获取用户资源
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ResultDataInfo<IdentityInfo> GetIdentity(string token);
        /// <summary>
        /// 获取用户资源
        /// </summary>
        /// <param name="token"></param>
        /// <param name="resourceCode">顶级资源的Id</param>
        /// <returns></returns>
        ResultDataInfo<IdentityInfo> GetIdentity(string token, string resourceCode);

        ResultInfo Verify(string token);
        ResultInfo Verify(string token, string resCode);

        /// <summary>
        /// 重新Loadcache数据
        /// </summary>
        /// <returns></returns>
        ResultDataInfo<bool> ReLoadCache();
    }
}
