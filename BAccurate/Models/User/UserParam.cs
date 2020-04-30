using BAccurate.Enums;
using System.Collections.Generic;

namespace BAccurate.Models.User
{
    public class UserParam
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户帐号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Slat
        /// </summary>
        public string Salt { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public string MobilePhone { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// 帐户类型
        /// </summary>
        public AccountType AccountType { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 扩展配置
        /// </summary>
        public string Config { get; set; }
        
        /// <summary>
        /// 角色列表
        /// </summary>
        public IList<int> Roles { get; set; }
    }
}
