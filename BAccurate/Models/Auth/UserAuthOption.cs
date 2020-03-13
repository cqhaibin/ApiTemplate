using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate.Models.Auth
{
    public class UserAuthOption
    {
        /// <summary>
        /// 用户登录过期时间，默认3600
        /// </summary>
        public int UserLoginExpireIn { get; set; }

        public UserAuthOption()
        {
            this.UserLoginExpireIn = 3600;
        }
    }
}
