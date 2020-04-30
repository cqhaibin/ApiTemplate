using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAccurate
{
    public static class OcmStatusCode
    {
        /// <summary>
        /// 账号被禁用
        /// </summary>
        public static int USER_FORBIDDEN = 200001;
        /// <summary>
        /// 账号密码错误
        /// </summary>
        public static int USER_PASSWORD_ERROR = 200002;

        /// <summary>
        /// 未能找到此账号
        /// </summary>
        public static int USER_NOT_FOUND = 200006;

        /// <summary>
        /// 账号未登录
        /// </summary>
        public static int USER_VERIFY_NOT_LOGIN = 200003;

        /// <summary>
        /// 账号过期
        /// </summary>
        public static int USER_VERIFY_EXPIRE = 200004;

        /// <summary>
        /// 账号无权限访问资源
        /// </summary>
        public static int USER_VERIFY_NO_ACCESS = 200005;

        /// <summary>
        /// 账号已存在
        /// </summary>
        public static int USER_ALREADY_EXIST = 200006;

        /// <summary>
        /// 未知状态
        /// </summary>
        public static int OCM_ERROR_UNKOWN = 200099;
    }
}
