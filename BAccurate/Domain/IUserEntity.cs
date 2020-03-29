using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAccurate.Models;
using BAccurate.Models.Auth;

namespace BAccurate.Domain
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public interface IUserEntity
    {
        UserInfo UserInfo { get; }

        string Token { get; }

        /// <summary>
        /// 客户端信息
        /// </summary>
        RequestClientInfo ClientInfo { get; }

        DateTime LoginTime { get; }

        DateTime ExpiredTime { get; }
        /// <summary>
        /// 用户登录配置
        /// </summary>
        UserAuthOption Option { get; }

        TokenEntity GetTokenEntity();

        bool Verify();

        bool Verify(string resCode);
    }
}
