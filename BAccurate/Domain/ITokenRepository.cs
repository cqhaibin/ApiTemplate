using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAM.Framework;

namespace BAccurate.Domain
{
    /// <summary>
    /// 认证、登录持久化接口
    /// </summary>
    public interface ITokenRepository:ITransient
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">用户Token实体</param>
        /// <param name="isLogin">登入或登出，默认登入</param>
        void InsertToken(TokenEntity entity, bool isLogin = true);

        IList<TokenEntity> GetAllTokens();

    }
}
